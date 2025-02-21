@ECHO off

IF "%~1"=="SN" GOTO sn
IF "%~1"=="BZ" GOTO bz
GOTO error

:bz
ECHO Game is BELOW ZERO
SET gamepath=E:\Games\Steam\steamapps\common\SubnauticaZero
SET deploypath=E:\Dev\DAG\Subnautica Mod Releases\SubnauticaZero
GOTO deploy

:sn
ECHO Game is SUBNAUTICA
SET gamepath=E:\Games\Steam\steamapps\common\Subnautica
SET deploypath=E:\Dev\DAG\Subnautica Mod Releases\Subnautica

:deploy
IF "%~2"=="" GOTO error
SET targetname=%~2

IF "%~3"=="" GOTO error
SET targetpath=%~3

IF "%~4"=="" GOTO error
SET targetdir=%~4

ECHO Deploying mod %targetname% from %targetpath%...
mkdir "%gamepath%\BepInEx\plugins\%targetname%"
mkdir "%gamepath%\BepInEx\plugins\%targetname%\Assets"
mkdir "%gamepath%\BepInEx\plugins\$%targetname%\Localization"

mkdir "%deploypath%"
mkdir "%deploypath%\%targetname%"
mkdir "%deploypath%\%targetname%\plugins"
mkdir "%deploypath%\%targetname%\plugins\%targetname%"
mkdir "%deploypath%\%targetname%\plugins\%targetname%\Assets"
mkdir "%deploypath%\%targetname%\plugins\%targetname%\Localization"

ECHO Deploy to game folder...
xcopy /q/y/i "%targetpath%" "%gamepath%\BepInEx\plugins\%targetname%"
xcopy /q/y/i "%targetdir%\Assets" "%gamepath%\BepInEx\plugins\%targetname%\Assets\*.*"
xcopy /q/y/i "%targetdir%\Localization" "%gamepath%\BepInEx\plugins\%targetname%\Localization\*.*"

ECHO Zip and deploying to Nexus folder...
xcopy /q/y/i "%targetpath%" "%deploypath%\%targetname%\plugins\%targetname%"
xcopy /q/y/i "%targetdir%\Assets" "%deploypath%\%targetname%\plugins\%targetname%\Assets\*.*"
xcopy /q/y/i "%targetdir%\Localization" "%deploypath%\%targetname%\plugins\%targetname%\Localization\*.*"

powershell Compress-Archive -Path '%deploypath%\%targetname%\plugins' -DestinationPath '%deploypath%\%targetname%\%targetname%.zip' -Force
rmdir /s /q "%deploypath%\%targetname%\plugins\"
ECHO Done!

GOTO end

:error
ECHO Invalid parameters!
ECHO USAGE:
ECHO Deploy.bat [SN/BZ] TargetName TargetPath TargetDir
GOTO end

:end

