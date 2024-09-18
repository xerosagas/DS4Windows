using DS4Windows;
using DS4WinWPF.DS4Forms.ViewModels;

namespace DS4WindowsTests;

[TestClass]
public class LightbarMacroTests
{
    [TestMethod]
    public void CheckCompile()
    {
        var binding = new OutBinding();
        binding.UseLightbarMacro = true;
        binding.LightbarMacroTrigger = LightbarMacroTrigger.Press;
        binding.LightbarMacro =
        [
            new LightbarMacroElement(new DS4Color(255, 255, 255), 100),
            new LightbarMacroElement(new DS4Color(255, 0, 0), 50),
            new LightbarMacroElement(new DS4Color(0, 128, 0), 1000),
            new LightbarMacroElement(new DS4Color(0, 0, 0), 5)
        ];

        var expectedStr = "True/255,255,255:100;255,0,0:50;0,128,0:1000;0,0,0:5/Press";
        var output = binding.CompileLightbarMacro();
        Assert.AreEqual(output, expectedStr);
    }

    [TestMethod]
    public void CheckParse()
    {
        var binding = new OutBinding();
        var incomingString = "True/255,255,255:100;255,0,0:50;0,128,0:1000;0,0,0:5/Press";
        binding.ParseLightbarMacro(incomingString);

        Assert.AreEqual(binding.UseLightbarMacro, true);
        Assert.AreEqual(binding.LightbarMacroTrigger, LightbarMacroTrigger.Press);
        var macro = new LightbarMacroElement[]
        {
            new(new DS4Color(255, 255, 255), 100),
            new(new DS4Color(255, 0, 0), 50),
            new(new DS4Color(0, 128, 0), 1000),
            new(new DS4Color(0, 0, 0), 5)
        };
        CollectionAssert.AreEqual(binding.LightbarMacro, macro);
    }
}