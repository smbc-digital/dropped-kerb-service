help:
	@cat ./MakefileHelp

# Project automation targets
# --------------------------

coverage:
	cd ./tests && dotnet test //p:CollectCoverage=true /p:Exclude=\"[*],[xunit*]*,[*]dropped_kerb_service-tests*\" //p:ExcludeByAttribute="ExcludeFromCodeCoverage" //p:CoverletOutputFormat=lcov

pipeline:
	./pipeline.sh

setup:
	./setup.sh