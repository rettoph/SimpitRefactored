@echo OFF

rem Used to create a SymLink between the given
rem %OUT_DIR% and %KSP_DIR%/GameData/KervalSimPit
rem If %KSP_DIR%/GameData/KervalSimPit already
rem exists this will do nothing. If need be,
rem delete any existing folders that pre-date
rem the creation of sumlink

set PROJECT_DIR=%1
set OUT_DIR=%2
set KSP_DIR=%3

rem --------------------------------------------
rem -               BEGIN SCRIPT               -
rem --------------------------------------------

set PROJECT_DIR=%PROJECT_DIR:"=%
set OUT_DIR=%OUT_DIR:"=%
set KSP_DIR=%KSP_DIR:"=%
set SYMLINK_TARGET=%KSP_DIR%\GameData\KerbalSimpit

if not exist "%PROJECT_DIR%" (
	echo Unable to locate PROJECT_DIR %PROJECT_DIR%
	exit /b 3 rem ERROR_PATH_NOT_FOUND
)

cd %PROJECT_DIR%
if not exist "%OUT_DIR%" (
	echo Unable to locate OUT_DIR %OUT_DIR%
	exit /b 3 rem ERROR_PATH_NOT_FOUND
)

if not exist "%KSP_DIR%" (
	echo Unable to locate KSP_DIR %KSP_DIR%
	exit /b 3 rem ERROR_PATH_NOT_FOUND
)

if exist "%SYMLINK_TARGET%" (
	echo %SYMLINK_TARGET% already exists. Deleting...
	
	rmdir "%SYMLINK_TARGET%"
)

echo Creating symlink from %OUT_DIR% to %SYMLINK_TARGET%...
mklink /j "%SYMLINK_TARGET%" "%OUT_DIR%"
exit /b %errorcode%