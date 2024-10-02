from pathlib import Path
import sys

project_dir = Path(sys.argv[1])
version = sys.argv[2]

# write the version to newest.txt
newest_txt = project_dir / "newest.txt"
with open(newest_txt, 'w') as file:
    file.write(version)
