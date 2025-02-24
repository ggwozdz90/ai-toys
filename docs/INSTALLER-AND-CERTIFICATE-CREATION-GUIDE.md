# Installer and Certificate Creation Guide

This document provides a detailed guide on how the installer for the AiToys project was created, including the configuration of the `appxmanifest` file, certificate generation, and the build process.

## Appxmanifest Configuration

The `Package.appxmanifest` file is included inside the `src\AiToys\` directory. Key elements include:

- **Identity**: Specifies the name, publisher, and version of the package.
- **Properties**: Defines display properties such as the display name, publisher display name, and logo.
- **Dependencies**: Lists the target device families and their versions.
- **Resources**: Specifies the language resources.
- **Applications**: Defines the application entry point, visual elements, and extensions.
- **Capabilities**: Lists the capabilities required by the application, such as internet access and full trust.

## Certificate Generation

To sign the MSIX package, a self-signed certificate is used. Follow these steps to generate and manage the certificate:

### Generate a Self-Signed Certificate

Replace `{company name}` and `{application name}` with your actual company name and application name.

```powershell
New-SelfSignedCertificate -Type Custom -Subject "CN={company name}" -KeyUsage DigitalSignature -FriendlyName "{application name}" -CertStoreLocation "Cert:\CurrentUser\My" -TextExtension @("2.5.29.37={text}1.3.6.1.5.5.7.3.3", "2.5.29.19={text}")
```

### Generate a Password

```powershell
$password = ConvertTo-SecureString -String "{password}" -Force -AsPlainText
```

### Export the Certificate

Replace `{thumbprint}` with the thumbprint of the generated certificate and `{file name}` with the desired filename for the PFX file.

```powershell
Export-PfxCertificate -cert "Cert:\CurrentUser\My\{thumbprint}" -FilePath "$PWD\{file name}.pfx" -Password $password
```

## Building the MSIX Package

Use the following command to build the MSIX package with the certificate. Replace `{path/to/pfx}` with the actual path to your PFX file and `{thumbprint}` with the thumbprint of the certificate.

```powershell
msbuild .\src\AiToys\AiToys.csproj -restore:True /p:Configuration=Release /p:Platform=x64 /p:PackageCertificateKeyFile="{path/to/pfx}" /p:PackageCertificateThumbprint="{thumbprint}" /p:GenerateAppxPackageOnBuild=True
```

## Managing the Certificate

To avoid publishing the PFX file in the Git repository, convert the certificate to a Base64 string and store it in GitHub Secrets. Follow these steps:

### Convert Certificate to Base64

```powershell
[Convert]::ToBase64String([IO.File]::ReadAllBytes("{path/to/pfx}")) | Out-File cert_base64.txt
```

### Export the Public Certificate

The public certificate is exported to allow users to install it on their machines, which helps in verifying the authenticity of the signed MSIX package. This step is crucial for users to trust the application and avoid warnings during installation.

```powershell
Export-Certificate -Cert (Get-ChildItem -Path "Cert:\CurrentUser\My\{thumbprint}") -FilePath "{file name}.cer"
```

### Importing the Certificate in CI

In your GitHub Actions workflow, decode the Base64 string and import the certificate:

```yaml
- name: Decode and save certificate
  run: |
    $certBytes = [Convert]::FromBase64String("${{ secrets.CODE_SIGNING_CERTIFICATE }}")
    $certPath = Join-Path -Path $env:RUNNER_TEMP -ChildPath "AiToys.pfx"
    [IO.File]::WriteAllBytes($certPath, $certBytes)
    echo "CERTIFICATE_PATH=$certPath" | Out-File -FilePath $env:GITHUB_ENV -Append
    $certPassword = ConvertTo-SecureString -String ${{ secrets.CODE_SIGNING_CERTIFICATE_PASSWORD }} -Force -AsPlainText
    Import-PfxCertificate -FilePath $certPath -CertStoreLocation Cert:\CurrentUser\My -Password $certPassword
  shell: pwsh

- name: Build MSIX
  run: |
    msbuild .\src\AiToys\AiToys.csproj `
      -restore:True `
      /p:Configuration=Release `
      /p:Platform=x64 `
      /p:PackageCertificateKeyFile=$env:CODE_SIGNING_CERTIFICATE_PATH \
      /p:PackageCertificateThumbprint=${{ secrets.CODE_SIGNING_CERTIFICATE_THUMBPRINT }} \
      /p:GenerateAppxPackageOnBuild=false
  shell: pwsh
```

### Installing the Public Certificate

To install the MSIX package, users need to install the public certificate on their machines. The public certificate is included in the ZIP file along with the MSIX package and a script to install the certificate.

The `Install-Certificate.ps1` script installs the public certificate.

## Note on Production Certificates

The self-signed certificate used here is for development purposes. For a production-ready application, a certificate from a trusted certificate authority should be purchased, which can be expensive. This guide focuses on creating an installable MSIX package for development and testing purposes.

## Table of Contents

- [Installer and Certificate Creation Guide](#installer-and-certificate-creation-guide)
  - [Appxmanifest Configuration](#appxmanifest-configuration)
  - [Certificate Generation](#certificate-generation)
    - [Generate a Self-Signed Certificate](#generate-a-self-signed-certificate)
    - [Generate a Password](#generate-a-password)
    - [Export the Certificate](#export-the-certificate)
  - [Building the MSIX Package](#building-the-msix-package)
  - [Managing the Certificate](#managing-the-certificate)
    - [Convert Certificate to Base64](#convert-certificate-to-base64)
    - [Export the Public Certificate](#export-the-public-certificate)
    - [Importing the Certificate in CI](#importing-the-certificate-in-ci)
    - [Installing the Public Certificate](#installing-the-public-certificate)
  - [Note on Production Certificates](#note-on-production-certificates)
  - [Table of Contents](#table-of-contents)
