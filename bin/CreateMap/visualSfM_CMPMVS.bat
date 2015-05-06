@ECHO OFF

IF [%1]==[] (
	@ECHO Usage path\VisualSfM.exe imagesDirPath
	@ECHO Result will be in imagesDirPath\_CMPMVS_00_OUT
	@ECHO OFF
) ELSE (
	%1 sfm+cmpmvs %2 %2\cmp.nvm
	CMPMVS.exe %2\cmp.nvm.cmp\00\mvs.ini
	md %2\_CMPMVS_00_OUT
	copy %2\cmp.nvm.cmp\00\data\_OUT %2\_CMPMVS_00_OUT
)