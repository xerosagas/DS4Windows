# DS4Windows

Like those other DS4 tools, but sexier.

DS4Windows is an extract anywhere program that allows you to get the best
DualShock 4 experience on your PC. By emulating an Xbox 360 controller, many
more games are accessible. Other input controllers are also supported including the
DualSense, Switch Pro, and JoyCon controllers (**first party hardware only**).

This project is a fork of the work of Jays2Kings and Ryochan7. It adds various new features like switch 
[debouncing](https://www.ganssle.com/debouncing.pdf), a tool that helps to fix stick drift and pitch and roll simulation
for DS3 based on accelerometer value (which is a work of [sunnyqeen](https://github.com/sunnyqeen)).

![DS4Windows Preview](https://raw.githubusercontent.com/Ryochan7/DS4Windows/jay/ds4winwpf_screen_20200412.png)

## About this fork

I've made this fork because some of the buttons on my controller started bouncing. Normally I would just add a
feature that would fix my problem, make a pull request to the original repo and forget about the project. 
The issue here is that Ryochan7 stopped maintaining the original project, so I decided to make slight 
modifications to the code that detects if the installed version is up-to-date, so it now pulls version info from my 
repo. This way if you install my version, you don't get the annoying popup saying your version is outdated. If there 
are any feature requests, I'm more than happy to at least look at them and assess whether I could add them.

## License

DS4Windows is licensed under the terms of the GNU General Public License version 3.
You can find a copy of the terms and conditions of that license at
[https://www.gnu.org/licenses/gpl-3.0.txt](https://www.gnu.org/licenses/gpl-3.0.txt). The license is also
available in this source code from the COPYING file.

## Downloads

- **[Main builds of DS4Windows](https://github.com/schmaldeo/DS4Windows/releases)**

## Install

You can install DS4Windows by downloading it from [releases](https://github.com/schmaldeo/DS4Windows/releases) and place it to your preferred place.

Alternatively, you can install [`ds4windows`](https://scoop.sh/#/apps?q=ds4windows&o=true&id=c8b519fcb06da6bb014569fd0a07521839ec5425) via [Scoop](https://scoop.sh/).

Alternatively, you can download [`ds4w.bat`](https://raw.githubusercontent.com/schmaldeo/DS4Windows/refs/heads/master/ds4w.bat) file and execute it. It will open a window that downloads and places the program in `%LOCALAPPDATA%\DS4Windows` and creates a desktop shortcut to the executable.

## Requirements

- Windows 10 or newer (Thanks Microsoft)
- Microsoft .NET 8.0 Desktop Runtime. [x64](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-8.0.0-windows-x64-installer) or [x86](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-8.0.0-windows-x86-installer)
- Visual C++ 2015-2022 Redistributable. [x64](https://aka.ms/vs/17/release/vc_redist.x64.exe) or [x86](https://aka.ms/vs/17/release/vc_redist.x86.exe)
- [ViGEmBus](https://vigem.org/) driver (DS4Windows will install it for you)
- **Sony** DualShock 4 or other supported controller
- Connection method:
  - Micro USB cable
  - [Sony Wireless Adapter](https://www.amazon.com/gp/product/B01KYVLKG2)
  - Bluetooth 4.0 (via an
  [adapter like this](https://www.newegg.com/Product/Product.aspx?Item=N82E16833166126)
  or built in pc). Only use of Microsoft BT stack is supported. CSR BT stack is
  confirmed to not work with the DS4 even though some CSR adapters work fine
  using Microsoft BT stack. Toshiba's adapters currently do not work.
  *Disabling 'Enable output data' in the controller profile settings might help with latency issues, but will disable lightbar and rumble support.*
- Disable **PlayStation Configuration Support** and
**Xbox Configuration Support** options in Steam
