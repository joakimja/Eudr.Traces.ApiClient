# Eudr.Traces.Integrations

**Eudr.Traces.Integrations** is a .NET 9 library that provides an abstraction layer for communicating with the EU TRACES system regarding EUDR (EU Deforestation Regulation) operations.

It defines the `IEUDRServiceAgent` interface, which enables retrieval, submission, and validation of Due Diligence Statements (DDS).
Contributions are very welcome – feel free to add more helpful methods or validations to improve the functionality of this library. 

---

## 📦 Installation

Install via NuGet.org:

```bash
dotnet add package Eudr.Traces.ApiClient
```

Or in your .csproj:

```xml
<PackageReference Include="Eudr.Traces.ApiClient" Version="*" />
```

---

## 🚀 Quick Example

```csharp
using Eudr.Traces.Integrations;
using Eudr.Traces.Integrations.Entities;

public class Demo
{
    private readonly IEUDRServiceAgent _agent;

    public Demo(IEUDRServiceAgent agent)
    {
        _agent = agent;
    }

    public async Task Run()
    {
        

        // Submit DDS with GPS
        var gpsRequest = new DDSWithGps { Latitude = 56.0, Longitude = 14.0 }; // and all othere properties needs
        var ddsId = await _agent.SubmitDdsAsync(gpsRequest);

        // Retrieve DDS status on an submitted and poll until you get reference & verification 
        var status = await _agent.GetDdsStatusAsync(new[] { Guid.Parse("11111111-1111-1111-1111-111111111111") });

        // Retrieve DDS by reference & verification see 
        var dds = await _agent.GetStatementByIdentifiersAsync("EU-2025-SE-000001", "VERIF-001234");

        // Validate DDS on a simple way
        var isValid = await _agent.ValidateDDSAsync("EU-2025-SE-000001", "VERIF-001234");
    }
}
```

---

## 📚 Features

- Retrieve DDS status by GUID identifiers
- Retrieve DDS by reference and verification numbers
- Submit DDS (full request, GPS-based, or reference-based)
- Validate DDS against defined business rules
- Abstraction layer over TRACES SOAP services
- .NET 9 compatible

---

## 🛠️ Target Framework

- .NET 9

---

## 💻 Repository

Source code and issue tracking:   
[GitHub - Eudr.Traces.ApiClient](https://github.com/joakimja/Eudr.Traces.ApiClient)

---

## 📜 License

This project is licensed under the MIT License. See the LICENSE file for details.
