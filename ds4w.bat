@echo off
setlocal

:: Check for admin privileges
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo This script requires administrative privileges. Please run as administrator.
    pause
    exit /b
)

:: Define the installation path
set INSTALL_PATH=C:\Program Files\DS4Windows

:: Function to prompt for version
:prompt_version
set /p VERSION="Enter the version (e.g., x.x.x): "
if "%VERSION%"=="" (
    echo Version cannot be empty. Please try again.
    goto prompt_version
)

:: Function to prompt for architecture
:prompt_arch
set /p ARCH="Enter the architecture (x64 or x86, default is x64): "
if "%ARCH%"=="" (
    set ARCH=x64
)

set BASE_URL=https://github.com/schmaldeo/DS4Windows/releases/download
set DOWNLOAD_URL=%BASE_URL%/v%VERSION%/DS4Windows_%VERSION%_%ARCH%.zip

:: Download the file
echo Downloading %DOWNLOAD_URL% ...
powershell -Command "Invoke-WebRequest -Uri '%DOWNLOAD_URL%' -OutFile 'DS4Windows_%VERSION%_%ARCH%.zip'"

if %errorLevel% neq 0 (
    echo Download failed. Please check the version and architecture.
    exit /b
)

echo Download completed.

:: Remove the previous version if it exists
if exist "%INSTALL_PATH%" (
    echo Removing previous version from %INSTALL_PATH%...
    rmdir /S /Q "%INSTALL_PATH%"
)

:: Unpack the ZIP file
echo Unpacking the ZIP file...
powershell -Command "Expand-Archive -Path 'DS4Windows_%VERSION%_%ARCH%.zip' -DestinationPath 'DS4Windows'"

:: Move the folder to Program Files
echo Moving DS4Windows folder to %INSTALL_PATH%...
move /Y "DS4Windows\DS4Windows" "%INSTALL_PATH%"

:: Create a shortcut on the desktop
set SHORTCUT_PATH="%USERPROFILE%\Desktop\DS4Windows.lnk"
powershell -Command "$s = New-Object -COMObject WScript.Shell; $shortcut = $s.CreateShortcut('%SHORTCUT_PATH%'); $shortcut.TargetPath = '%INSTALL_PATH%\DS4Windows.exe'; $shortcut.IconLocation = '%INSTALL_PATH%\DS4Windows.exe'; $shortcut.Save()"

:: Clean up downloaded and unpacked files
echo Cleaning up...
del /Q "DS4Windows_%VERSION%_%ARCH%.zip"
rmdir /S /Q "DS4Windows"

echo Installation completed.

:: Wait for user input before closing
echo Press any key to exit...
pause >nul
endlocal
