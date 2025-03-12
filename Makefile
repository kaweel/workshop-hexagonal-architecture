# Set default TimeZone
TZ ?= UTC
SCRIPTNAME ?= DEFAULT_SCRIPT

# Start the application
start-app:
	@echo "🚀 Starting application with TimeZone=$(TZ)..."
	dotnet run --project APIHost
# Run all tests with verbose output
test:
	@echo "🧪 Running all tests..."
	ASPNETCORE_ENVIRONMENT=Test dotnet test
unit-test:
	@echo "🧪 Running unit tests..."
	ASPNETCORE_ENVIRONMENT=Test dotnet test --filter Category=UnitTest
integration-test:
	@echo "🧪 Running integration  tests..."
	ASPNETCORE_ENVIRONMENT=Test dotnet test --filter Category=IntegrationTest
test-cov:
	@echo "🧪 Test and Generate test coverage report"
	rm -rf .coverageReport \
	&& rm -rf ./*Test/TestResults \
	&& ASPNETCORE_ENVIRONMENT=Test dotnet test --collect:"XPlat Code Coverage" \
	&& reportgenerator -reports:"./*/TestResults/*/coverage.cobertura.xml" -targetdir:".coverageReport" -reporttypes:"Html" \
	&& open .coverageReport/index.html
gen-sql:
	@echo "🧪 Generate migration sql script ${SCRIPTNAME}"
	dotnet ef migrations add ${SCRIPTNAME} --project Infrastructure --startup-project APIHost