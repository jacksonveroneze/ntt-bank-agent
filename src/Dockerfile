# =====================================================
# Build + Publish
# =====================================================
FROM mcr.microsoft.com/dotnet/sdk:10.0-noble AS publish

WORKDIR /src

COPY ./main/Api/Api.csproj ./main/Api/
COPY ./main/Application/Application.csproj ./main/Application/
COPY ./main/Domain/Domain.csproj ./main/Domain/
COPY ./main/Infrastructure/Infrastructure.csproj ./main/Infrastructure/
COPY *.slnx ./
COPY nuget.config ./
COPY Directory.Build.props ./
COPY Directory.Packages.props ./

RUN dotnet restore ./main/Api/Api.csproj \
    -r linux-x64 \
    -p:PublishReadyToRun=true

COPY . .

RUN dotnet publish ./main/Api/Api.csproj \
    -c Release \
    -r linux-x64 \
    --self-contained false \
    --no-restore \
    -p:PublishReadyToRun=true \
    -p:PublishTrimmed=false \
    -p:DebugType=None \
    -p:DebugSymbols=false \
    --output /publish

# =====================================================
# Etapa Final: Imagem de Runtime
# =====================================================
FROM jacksonveroneze/aspnet:10.0-alpine-1.5.0

ENV DOTNET_GCDynamicAdaptationMode=1 \
    DOTNET_GCHeapHardLimitPercent=0x4B \
    DOTNET_TieredCompilation=1 \
    DOTNET_TC_QuickJitForLoops=1

ENV ASSEMBLY_NAME=/home/app/Api.dll

COPY --from=publish /publish /home/app
