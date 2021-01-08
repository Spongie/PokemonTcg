``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.685 (2004/?/20H1)
Intel Core i5-8600K CPU 3.60GHz (Coffee Lake), 1 CPU, 6 logical and 6 physical cores
.NET Core SDK=5.0.101
  [Host]     : .NET Core 3.1.10 (CoreCLR 4.700.20.51601, CoreFX 4.700.20.51901), X64 RyuJIT
  DefaultJob : .NET Core 3.1.10 (CoreCLR 4.700.20.51601, CoreFX 4.700.20.51901), X64 RyuJIT


```
|         Method |      Mean |     Error |    StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|--------------- |----------:|----------:|----------:|-------:|------:|------:|----------:|
| FirstOrDefault | 71.501 ns | 0.3885 ns | 0.3444 ns | 0.0272 |     - |     - |     128 B |
|          Array |  1.958 ns | 0.0068 ns | 0.0064 ns |      - |     - |     - |         - |
