cd %~dp0
mkdir dist

call:copyBin compiler\gpc\bin\Debug
call:copyBin tools\FPG2GPK\bin\Debug
call:copyBin tools\FNT2GPK\bin\Debug
call:copyBin IDE\GPIDE\bin\Debug
call:copyBin tools\GPKEditor\bin\Debug
call:copyBin tools\Littera2GPK\bin\Debug
del dist\*.vshost.exe

rmdir /s /q dist\runtime
mkdir dist\runtime
xcopy /s /e /y runtime\* dist\runtime
copy resources\fontinstall.vbs dist

goto :eof

:copyBin
	SET "SRC=%~1"
	copy %SRC%\*.exe dist
	copy %SRC%\*.dll dist
	copy %SRC%\*.ini dist
	copy %SRC%\*.ttf dist
goto :eof

:eof
