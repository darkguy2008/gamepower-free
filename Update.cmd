cd %~dp0
mkdir dist

call:copyBin compiler\gpc\bin\Debug
call:copyBin tools\FPG2GPK\bin\Debug
call:copyBin tools\FNT2GPK\bin\Debug
call:copyBin IDE\GPIDE\bin\Debug
call:copyBin tools\GPKEditor\bin\Debug
call:copyBin tools\Littera2GPK\bin\Debug
call:copyBin tools\DIV2Help2HTML\bin\Debug
del dist\*.vshost.exe

rmdir /s /q dist\runtime
mkdir dist\runtime
xcopy /s /e /y runtime\* dist\runtime
copy resources\fontinstall.vbs dist

call:copyDir examples examples
call:copyDir docs docs

goto :eof

:copyBin
	SET "SRC=%~1"
	copy %SRC%\*.exe dist
	copy %SRC%\*.dll dist
	copy %SRC%\*.ttf dist
goto :eof

:copyDir
	SET "DST=%~1"
	SET "SRC=%~2"
	mkdir dist\%DST%
	xcopy /s /e /y %SRC%\* dist\%DST%
goto :eof

:eof
