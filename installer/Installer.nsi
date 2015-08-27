!define APPNAME "GamePower FREE version"
!define COMPANYNAME "Alemar/DARKGuy"
!define DESCRIPTION "GamePower FREE version"

!define VERSIONMAJOR 0
!define VERSIONMINOR 8
!define VERSIONBUILD 2

!define HELPURL "http://gamepower.no-ip.org/"
!define UPDATEURL "http://gamepower.no-ip.org/"
!define ABOUTURL "http://gamepower.no-ip.org/"

!define INSTALLSIZE 3850

RequestExecutionLevel admin
InstallDir "C:\GamePower"

# rtf or txt file - remember if it is txt, it must be in the DOS text format (\r\n)
LicenseData "..\resources\License.rtf"
# This will be in the installer/uninstaller's title bar
Name "${APPNAME}"
Icon "app.ico"
outFile "..\GamePower FREE ${VERSIONMAJOR}.${VERSIONMINOR}.${VERSIONBUILD}.exe"

!include LogicLib.nsh

# Just three pages - license agreement, install location, and installation
page license
page directory
Page instfiles

!macro VerifyUserIsAdmin
	UserInfo::GetAccountType
	pop $0
	${If} $0 != "admin" ;Require admin rights on NT4+
	        messageBox mb_iconstop "This installer requires elevated privileges!"
	        setErrorLevel 740 ;ERROR_ELEVATION_REQUIRED
	        quit
	${EndIf}
!macroend

function .onInit
	setShellVarContext all
	!insertmacro VerifyUserIsAdmin
functionEnd

section "install"
	setOutPath $INSTDIR

	File /r ..\dist\*
	ExecWait 'cscript "$INSTDIR\FontInstall.vbs"'
	Delete "$INSTDIR\FontInstall.vbs"
	Delete "$INSTDIR\FSEX300.ttf"

	writeUninstaller "$INSTDIR\Uninstall.exe"
sectionEnd

# Uninstaller

function un.onInit
	SetShellVarContext all

	#Verify the uninstaller - last chance to back out
	MessageBox MB_OKCANCEL "Are you sure you want to uninstall ${APPNAME}?" IDOK next
		Abort
	next:
	!insertmacro VerifyUserIsAdmin
functionEnd

section "uninstall"

	# Remove files
	rmDir /r "$INSTDIR\*"
	delete $INSTDIR\Uninstall.exe
	rmDir $INSTDIR

sectionEnd
