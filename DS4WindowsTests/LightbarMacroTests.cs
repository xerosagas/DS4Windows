using DS4Windows;
using DS4WinWPF.DS4Forms.ViewModels;

namespace DS4WindowsTests;

[TestClass]
public class LightbarMacroTests
{
    [TestMethod]
    public void CheckCompile()
    {
        var macro = new LightbarMacro(
            true,
            [
                new LightbarMacroElement(new DS4Color(255, 255, 255), 100),
                new LightbarMacroElement(new DS4Color(255, 0, 0), 50),
                new LightbarMacroElement(new DS4Color(0, 128, 0), 1000),
                new LightbarMacroElement(new DS4Color(0, 0, 0), 5)
            ],
            LightbarMacroTrigger.Press);
        var expectedStr = "True/255,255,255:100;255,0,0:50;0,128,0:1000;0,0,0:5/Press";
        var output = macro.Compile();
        Assert.AreEqual(output, expectedStr);
    }

    [TestMethod]
    public void CheckParse()
    {
        var incomingString = "True/255,255,255:100;255,0,0:50;0,128,0:1000;0,0,0:5/Press";
        var macro = new LightbarMacro();
        macro.Parse(incomingString);

        Assert.AreEqual(macro.Active, true);
        Assert.AreEqual(macro.Trigger, LightbarMacroTrigger.Press);
        var expected = new LightbarMacroElement[]
        {
            new(new DS4Color(255, 255, 255), 100),
            new(new DS4Color(255, 0, 0), 50),
            new(new DS4Color(0, 128, 0), 1000),
            new(new DS4Color(0, 0, 0), 5)
        };
        CollectionAssert.AreEqual(macro.Macro, expected);
        CollectionAssert.AreEqual(macro.ObservableMacro, expected);
    }
}