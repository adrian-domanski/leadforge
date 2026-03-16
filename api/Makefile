test:
	dotnet test

coverage:
	dotnet test tests/LeadForge.UnitTests /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
	reportgenerator -reports:tests/LeadForge.UnitTests/coverage.cobertura.xml -targetdir:coverage-report
	open coverage-report/index.html

clean:
	rm -rf coverage-report
	rm -rf **/coverage.cobertura.xml