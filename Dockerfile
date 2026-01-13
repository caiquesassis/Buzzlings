# --- STAGE 1: RUNTIME BASE ---
# Downloads a slim version of .NET 9 that ONLY contains the engine to run apps.
# It does not contain the compiler, which keeps the final image small and secure.
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release

# Sets the internal folder where the app will live inside the container.
WORKDIR /src

# We copy the Solution file first. This is the "map" for all your projects.
COPY ["Buzzlings.sln", "./"]

# We copy every .csproj file into its specific folder structure. 
# Docker caches these layers. If your NuGet packages don't change, 
# Docker skips the 'restore' step next time, making builds much faster.
COPY ["src/Buzzlings/Buzzlings.Web.csproj", "src/Buzzlings/"]
COPY ["src/Buzzlings.Api/Buzzlings.Api.csproj", "src/Buzzlings.Api/"]
COPY ["src/Buzzlings.BusinessLogic/Buzzlings.BusinessLogic.csproj", "src/Buzzlings.BusinessLogic/"]
COPY ["src/Buzzlings.Data/Buzzlings.Data.csproj", "src/Buzzlings.Data/"]
COPY ["tests/Buzzlings.Tests/Buzzlings.Tests.csproj", "tests/Buzzlings.Tests/"]

# Download all NuGet dependencies for the entire solution.
RUN dotnet restore "./Buzzlings.sln"

# Now we copy all the actual source code (.cs, .cshtml, etc.) into the container.
COPY . .

# Compile the entire solution in Release mode.
# We use --no-restore because we just did it in the step above.
RUN dotnet build "./Buzzlings.sln" -c $BUILD_CONFIGURATION --no-restore

# --- STAGE 2: TEST VALIDATION (The "Safety Gate") ---
# This stage runs your unit tests. If any test fails, the build stops here.
# It uses the 'build' stage results to avoid re-compiling.
FROM build AS test
RUN dotnet test "./Buzzlings.sln" -c $BUILD_CONFIGURATION --no-build --verbosity normal

# --- STAGE 3: PUBLISH (The "Packager") ---
# This takes the compiled code and creates the final "ready-to-run" DLLs.
# This stage only runs if the 'test' stage above passed successfully.
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "src/Buzzlings/Buzzlings.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# --- STAGE 4: FINAL RUNTIME (The "Finished Product") ---
# We switch to the 'aspnet' image, which is much smaller and more secure 
# because it doesn't contain the SDK or your source code—just the .NET runtime.
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# We copy ONLY the finished DLLs from the 'publish' stage into this final image.
COPY --from=publish /app/publish .

# Informs Docker that the app listens on these ports. 
# 8080 is the default for .NET 8/9 containers.
# This must be set here otherwise it's dropped by previous steps.
EXPOSE 8080

# This tells the container what to do when it starts up.
# It runs your Web project, which handles the UI and the API.
ENTRYPOINT ["dotnet", "Buzzlings.Web.dll"]