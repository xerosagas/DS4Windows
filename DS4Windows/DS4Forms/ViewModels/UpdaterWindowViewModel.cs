/*
DS4Windows
Copyright (C) 2023  Travis Nickles

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Text.Json;
using System.Text.Json.Serialization;
using HttpProgress;
using MarkdownEngine = MdXaml.Markdown;

namespace DS4WinWPF.DS4Forms.ViewModels
{
    class UpdaterWindowViewModel
    {
        public const string CHANGELOG_URI = "https://raw.githubusercontent.com/Ryochan7/DS4Windows/jay/DS4Windows/Changelog.min.json";

        private string Version { get; set; }

        private FlowDocument changelogDocument;
        public FlowDocument ChangelogDocument
        {
            get => changelogDocument;
            private set
            {
                if (changelogDocument == value) return;
                changelogDocument = value;
                ChangelogDocumentChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler ChangelogDocumentChanged;


        public UpdaterWindowViewModel(string version)
        {
            BuildTempDocument("Retrieving changelog info.Please wait...");
            Version = version;
        }

        public void BuildChangelogDocument(Dictionary<Version, string> versions)
        {
            MarkdownEngine engine = new();
            FlowDocument flow = new();

            foreach (var version in versions)
            {
                Paragraph paragraph = new();
                paragraph.Inlines.Add(new Run($"Version {version.Key.ToString()}") { Tag = "Header" });
                flow.Blocks.Add(paragraph);

                var parsedChangelog = ParseChangelogString(version.Value);
                var doc = engine.Transform(parsedChangelog);
                flow.Blocks.AddRange(new List<Block>(doc.Blocks));
            }

            ChangelogDocument = flow;
        }

        private static string ParseChangelogString(string changelog)
        {
            var split = changelog.Split("\n").ToList();
            split.RemoveAll(x => x.StartsWith("**Full Changelog**"));
            return string.Join(Environment.NewLine, split);
        }

        private void BuildTempDocument(string message)
        {
            FlowDocument flow = new FlowDocument();
            flow.Blocks.Add(new Paragraph(new Run(message)));
            flow.Blocks.Add(new Paragraph(new Run(message)));
            ChangelogDocument = flow;
        }

        public void SetSkippedVersion()
        {
            if (!string.IsNullOrEmpty(Version))
            {
                DS4Windows.Global.LastVersionChecked = Version;
            }
        }

        public void BlankSkippedVersion()
        {
            DS4Windows.Global.LastVersionChecked = string.Empty;
        }
    }

    // all of these below are useless
    public class ChangelogInfo
    {
        private string latestVersion;
        private ChangeVersionNumberInfo latestVersionInfo;
        private DateTime updatedAt;
        private ChangelogVersions changelog;

        [JsonPropertyName("latest_version")]
        public string LatestVersion { get => latestVersion; set => latestVersion = value; }


        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get => updatedAt; set => updatedAt = value; }

        [JsonPropertyName("changelog")]
        public ChangelogVersions Changelog { get => changelog; set => changelog = value; }

        [JsonPropertyName("latest_version_number_info")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public ChangeVersionNumberInfo LatestVersionInfo
        {
            get => latestVersionInfo;
            set => latestVersionInfo = value;
        }
    }

    public class ChangeVersionNumberInfo
    {
        private ushort majorPart;
        private ushort minorPart;
        private ushort buildPart;
        private ushort privatePart;

        [JsonPropertyName("majorPart")]
        public ushort MajorPart { get => majorPart; set => majorPart = value; }

        [JsonPropertyName("minorPart")]
        public ushort MinorPart { get => minorPart; set => minorPart = value; }

        [JsonPropertyName("buildPart")]
        public ushort BuildPart { get => buildPart; set => buildPart = value; }

        [JsonPropertyName("privatePart")]
        public ushort PrivatePart { get => privatePart; set => privatePart = value; }

        public ulong GetVersionNumber()
        {
            ulong temp = (ulong)majorPart << 48 | (ulong)minorPart << 32 |
                (ulong)buildPart << 16 | privatePart;
            return temp;
        }
    }

    public class ChangelogVersions
    {
        private List<ChangeVersionInfo> versions;

        [JsonPropertyName("versions")]
        public List<ChangeVersionInfo> Versions { get => versions; set => versions = value; }
    }

    public class ChangeVersionInfo
    {
        private string version;
        private ChangeVersionNumberInfo versionNumberInfo;
        private string baseHeader;
        private DateTime releaseDate;
        private List<VersionLogLocale> versionLocales;

        [JsonPropertyName("version_str")]
        public string Version { get => version; set => version = value; }

        [JsonPropertyName("base_header")]
        public string BaseHeader { get => baseHeader; set => baseHeader = value; }

        [JsonPropertyName("release_date")]
        public DateTime ReleaseDate { get => releaseDate; set => releaseDate = value; }

        [JsonPropertyName("locales")]
        public List<VersionLogLocale> VersionLocales { get => versionLocales; set => versionLocales = value; }

        [JsonPropertyName("version_number_info")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public ChangeVersionNumberInfo VersionNumberInfo
        {
            get => versionNumberInfo; set => versionNumberInfo = value;
        }

        public VersionLogLocale ApplicableInfo(string culture)
        {
            Dictionary<string, VersionLogLocale> tempDict =
                new Dictionary<string, VersionLogLocale>();

            foreach (VersionLogLocale logLoc in versionLocales)
            {
                tempDict.Add(logLoc.Code, logLoc);
            }

            VersionLogLocale result = null;
            CultureInfo hairyLegs = null;
            try
            {
                if (!string.IsNullOrEmpty(culture))
                {
                    hairyLegs = CultureInfo.GetCultureInfo(culture);
                }
            }
            catch (CultureNotFoundException) { }

            if (hairyLegs != null)
            {
                if (tempDict.ContainsKey(hairyLegs.Name))
                {
                    result = tempDict[hairyLegs.Name];
                }
                else if (tempDict.ContainsKey(hairyLegs.TwoLetterISOLanguageName))
                {
                    result =
                        tempDict[hairyLegs.TwoLetterISOLanguageName];
                }
            }

            if (result == null && versionLocales.Count > 0)
            {
                // Default to first entry if specific culture info not found
                result = versionLocales[0];
            }

            return result;
        }
    }

    public class VersionLogLocale
    {
        private string code;
        private string header;
        private List<string> logText;
        private string editor;
        private List<string> editorsNote;
        private DateTime editedAt;

        private string displayLogText;
        public string DisplayLogText { get => displayLogText; }

        public string Code { get => code; set => code = value; }
        public string Header { get => header; set => header = value; }

        [JsonPropertyName("log_text")]
        public List<string> LogText
        {
            get => logText;
            set
            {
                logText = value;
            }
        }

        [JsonPropertyName("editor")]
        public string Editor { get => editor; set => editor = value; }

        [JsonPropertyName("editors_note")]
        public List<string> EditorsNote { get => editorsNote; set => editorsNote = value; }

        [JsonPropertyName("updated_at")]
        public DateTime EditedAt { get => editedAt; set => editedAt = value; }

        public void BuildDisplayText()
        {
            displayLogText = string.Join("\n", logText);
        }
    }
}
