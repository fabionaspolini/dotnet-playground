# microsoft-logging-benchmark-playground


## MyJsonConsole -> SimpleLog (ShortRunJob)

|    Method | LoggerProvider | Scopes | Activity |     Mean |    Error |  StdDev |  StdErr |      Min |       Q1 |   Median |       Q3 |      Max |    Op/s |
|---------- |--------------- |------- |--------- |---------:|---------:|--------:|--------:|---------:|---------:|---------:|---------:|---------:|--------:|
| SimpleLog |         MyJson |  False |    False | 122.1 μs | 38.77 μs | 2.13 μs | 1.23 μs | 120.1 μs | 120.9 μs | 121.7 μs | 123.0 μs | 124.3 μs | 8,193.0 |
| SimpleLog |         MyJson |  False |     True | 179.9 μs | 39.69 μs | 2.18 μs | 1.26 μs | 178.6 μs | 178.6 μs | 178.7 μs | 180.5 μs | 182.4 μs | 5,559.3 |
| SimpleLog |         MyJson |   True |    False | 156.6 μs | 19.74 μs | 1.08 μs | 0.62 μs | 156.0 μs | 156.0 μs | 156.1 μs | 157.0 μs | 157.9 μs | 6,383.9 |
| SimpleLog |         MyJson |   True |     True | 198.5 μs | 11.68 μs | 0.64 μs | 0.37 μs | 198.1 μs | 198.1 μs | 198.1 μs | 198.7 μs | 199.2 μs | 5,038.7 |

## MyJsonConsole -> All tests (ShortRunJob)

|          Method | LoggerProvider | Scopes | Activity |     Mean |    Error |  StdDev |  StdErr |      Min |       Q1 |   Median |       Q3 |      Max |    Op/s |
|---------------- |--------------- |------- |--------- |---------:|---------:|--------:|--------:|---------:|---------:|---------:|---------:|---------:|--------:|
|       **SimpleLog** |         **MyJson** |  **False** |    **False** | **122.2 μs** | **37.31 μs** | **2.05 μs** | **1.18 μs** | **120.0 μs** | **121.2 μs** | **122.4 μs** | **123.2 μs** | **124.1 μs** | **8,185.4** |
|  OneTemplateLog |         MyJson |  False |    False | 160.5 μs | 21.71 μs | 1.19 μs | 0.69 μs | 159.4 μs | 159.9 μs | 160.4 μs | 161.1 μs | 161.8 μs | 6,229.6 |
| FiveTemplateLog |         MyJson |  False |    False | 161.1 μs | 21.72 μs | 1.19 μs | 0.69 μs | 159.8 μs | 160.5 μs | 161.2 μs | 161.7 μs | 162.2 μs | 6,207.7 |
|       **SimpleLog** |         **MyJson** |  **False** |     **True** | **175.5 μs** | **25.02 μs** | **1.37 μs** | **0.79 μs** | **174.1 μs** | **174.8 μs** | **175.5 μs** | **176.2 μs** | **176.9 μs** | **5,698.1** |
|  OneTemplateLog |         MyJson |  False |     True | 200.8 μs | 18.08 μs | 0.99 μs | 0.57 μs | 200.1 μs | 200.2 μs | 200.3 μs | 201.1 μs | 201.9 μs | 4,980.3 |
| FiveTemplateLog |         MyJson |  False |     True | 200.9 μs | 11.81 μs | 0.65 μs | 0.37 μs | 200.3 μs | 200.5 μs | 200.7 μs | 201.1 μs | 201.6 μs | 4,978.7 |
|       **SimpleLog** |         **MyJson** |   **True** |    **False** | **157.7 μs** | **23.28 μs** | **1.28 μs** | **0.74 μs** | **156.6 μs** | **157.0 μs** | **157.5 μs** | **158.3 μs** | **159.1 μs** | **6,340.7** |
|  OneTemplateLog |         MyJson |   True |    False | 173.5 μs | 21.56 μs | 1.18 μs | 0.68 μs | 172.8 μs | 172.8 μs | 172.8 μs | 173.8 μs | 174.8 μs | 5,764.3 |
| FiveTemplateLog |         MyJson |   True |    False | 172.9 μs |  8.29 μs | 0.45 μs | 0.26 μs | 172.5 μs | 172.6 μs | 172.7 μs | 173.1 μs | 173.4 μs | 5,784.7 |
|       **SimpleLog** |         **MyJson** |   **True** |     **True** | **199.7 μs** | **12.46 μs** | **0.68 μs** | **0.39 μs** | **199.0 μs** | **199.4 μs** | **199.7 μs** | **200.1 μs** | **200.4 μs** | **5,006.7** |
|  OneTemplateLog |         MyJson |   True |     True | 211.6 μs | 45.40 μs | 2.49 μs | 1.44 μs | 208.7 μs | 210.7 μs | 212.7 μs | 213.0 μs | 213.3 μs | 4,726.0 |
| FiveTemplateLog |         MyJson |   True |     True | 227.3 μs | 33.40 μs | 1.83 μs | 1.06 μs | 225.6 μs | 226.3 μs | 227.0 μs | 228.1 μs | 229.2 μs | 4,400.2 |

## All providers -> All tests (ShortRunJob)

|          Method | LoggerProvider | Scopes | Activity |     Mean |     Error |  StdDev |  StdErr |      Min |       Q1 |   Median |       Q3 |      Max |    Op/s |
|---------------- |--------------- |------- |--------- |---------:|----------:|--------:|--------:|---------:|---------:|---------:|---------:|---------:|--------:|
|       **SimpleLog** |        **Console** |  **False** |    **False** | **187.2 μs** |  **17.56 μs** | **0.96 μs** | **0.56 μs** | **186.5 μs** | **186.7 μs** | **186.8 μs** | **187.6 μs** | **188.3 μs** | **5,340.6** |
|  OneTemplateLog |        Console |  False |    False | 190.0 μs |  46.76 μs | 2.56 μs | 1.48 μs | 187.2 μs | 188.9 μs | 190.5 μs | 191.4 μs | 192.2 μs | 5,263.9 |
| FiveTemplateLog |        Console |  False |    False | 191.2 μs |  22.62 μs | 1.24 μs | 0.72 μs | 190.4 μs | 190.5 μs | 190.5 μs | 191.6 μs | 192.6 μs | 5,230.5 |
|       **SimpleLog** |        **Console** |  **False** |     **True** | **298.5 μs** |  **27.59 μs** | **1.51 μs** | **0.87 μs** | **296.8 μs** | **297.8 μs** | **298.9 μs** | **299.3 μs** | **299.8 μs** | **3,350.4** |
|  OneTemplateLog |        Console |  False |     True | 300.6 μs |  12.53 μs | 0.69 μs | 0.40 μs | 299.8 μs | 300.3 μs | 300.9 μs | 301.0 μs | 301.1 μs | 3,326.6 |
| FiveTemplateLog |        Console |  False |     True | 299.0 μs |  44.72 μs | 2.45 μs | 1.42 μs | 296.4 μs | 297.8 μs | 299.1 μs | 300.2 μs | 301.3 μs | 3,344.9 |
|       **SimpleLog** |        **Console** |   **True** |    **False** | **297.7 μs** |  **97.97 μs** | **5.37 μs** | **3.10 μs** | **294.6 μs** | **294.6 μs** | **294.6 μs** | **299.3 μs** | **303.9 μs** | **3,359.0** |
|  OneTemplateLog |        Console |   True |    False | 294.0 μs |  41.22 μs | 2.26 μs | 1.30 μs | 291.8 μs | 292.8 μs | 293.8 μs | 295.1 μs | 296.3 μs | 3,401.3 |
| FiveTemplateLog |        Console |   True |    False | 296.0 μs | 164.63 μs | 9.02 μs | 5.21 μs | 290.1 μs | 290.8 μs | 291.5 μs | 298.9 μs | 306.4 μs | 3,378.4 |
|       **SimpleLog** |        **Console** |   **True** |     **True** | **319.0 μs** |  **51.07 μs** | **2.80 μs** | **1.62 μs** | **316.5 μs** | **317.5 μs** | **318.4 μs** | **320.2 μs** | **322.0 μs** | **3,135.0** |
|  OneTemplateLog |        Console |   True |     True | 317.2 μs |  20.44 μs | 1.12 μs | 0.65 μs | 316.3 μs | 316.6 μs | 317.0 μs | 317.7 μs | 318.5 μs | 3,152.2 |
| FiveTemplateLog |        Console |   True |     True | 311.9 μs |  39.57 μs | 2.17 μs | 1.25 μs | 309.8 μs | 310.8 μs | 311.8 μs | 313.0 μs | 314.2 μs | 3,205.9 |
|       **SimpleLog** |  **SimpleConsole** |  **False** |    **False** | **103.3 μs** |  **66.03 μs** | **3.62 μs** | **2.09 μs** | **100.9 μs** | **101.3 μs** | **101.6 μs** | **104.6 μs** | **107.5 μs** | **9,676.4** |
|  OneTemplateLog |  SimpleConsole |  False |    False | 103.3 μs |   3.42 μs | 0.19 μs | 0.11 μs | 103.1 μs | 103.2 μs | 103.4 μs | 103.4 μs | 103.4 μs | 9,679.7 |
| FiveTemplateLog |  SimpleConsole |  False |    False | 103.7 μs |   6.36 μs | 0.35 μs | 0.20 μs | 103.5 μs | 103.5 μs | 103.5 μs | 103.8 μs | 104.1 μs | 9,646.4 |
|       **SimpleLog** |  **SimpleConsole** |  **False** |     **True** | **142.0 μs** |  **94.26 μs** | **5.17 μs** | **2.98 μs** | **136.9 μs** | **139.4 μs** | **141.9 μs** | **144.5 μs** | **147.2 μs** | **7,043.1** |
|  OneTemplateLog |  SimpleConsole |  False |     True | 138.1 μs |  38.21 μs | 2.09 μs | 1.21 μs | 135.8 μs | 137.2 μs | 138.5 μs | 139.3 μs | 140.0 μs | 7,240.3 |
| FiveTemplateLog |  SimpleConsole |  False |     True | 140.4 μs |   4.90 μs | 0.27 μs | 0.16 μs | 140.1 μs | 140.3 μs | 140.5 μs | 140.5 μs | 140.6 μs | 7,123.3 |
|       **SimpleLog** |  **SimpleConsole** |   **True** |    **False** | **126.6 μs** |   **9.85 μs** | **0.54 μs** | **0.31 μs** | **126.1 μs** | **126.3 μs** | **126.5 μs** | **126.8 μs** | **127.2 μs** | **7,900.2** |
|  OneTemplateLog |  SimpleConsole |   True |    False | 125.9 μs |  20.97 μs | 1.15 μs | 0.66 μs | 124.7 μs | 125.3 μs | 126.0 μs | 126.5 μs | 127.0 μs | 7,942.7 |
| FiveTemplateLog |  SimpleConsole |   True |    False | 126.0 μs |  21.37 μs | 1.17 μs | 0.68 μs | 124.6 μs | 125.6 μs | 126.5 μs | 126.6 μs | 126.8 μs | 7,938.1 |
|       **SimpleLog** |  **SimpleConsole** |   **True** |     **True** | **169.9 μs** |  **12.53 μs** | **0.69 μs** | **0.40 μs** | **169.2 μs** | **169.6 μs** | **170.1 μs** | **170.3 μs** | **170.5 μs** | **5,884.8** |
|  OneTemplateLog |  SimpleConsole |   True |     True | 171.6 μs |  27.08 μs | 1.48 μs | 0.86 μs | 169.9 μs | 171.1 μs | 172.3 μs | 172.5 μs | 172.6 μs | 5,826.0 |
| FiveTemplateLog |  SimpleConsole |   True |     True | 171.2 μs |  21.35 μs | 1.17 μs | 0.68 μs | 169.9 μs | 170.8 μs | 171.7 μs | 171.9 μs | 172.0 μs | 5,841.0 |
|       **SimpleLog** |           **Json** |  **False** |    **False** | **176.8 μs** |  **46.41 μs** | **2.54 μs** | **1.47 μs** | **174.8 μs** | **175.4 μs** | **176.1 μs** | **177.9 μs** | **179.7 μs** | **5,654.7** |
|  OneTemplateLog |           Json |  False |    False | 177.9 μs |  25.98 μs | 1.42 μs | 0.82 μs | 176.7 μs | 177.1 μs | 177.5 μs | 178.5 μs | 179.5 μs | 5,620.7 |
| FiveTemplateLog |           Json |  False |    False | 202.5 μs | 119.65 μs | 6.56 μs | 3.79 μs | 198.5 μs | 198.7 μs | 198.9 μs | 204.5 μs | 210.1 μs | 4,938.3 |
|       **SimpleLog** |           **Json** |  **False** |     **True** | **246.6 μs** |  **73.91 μs** | **4.05 μs** | **2.34 μs** | **243.5 μs** | **244.3 μs** | **245.1 μs** | **248.1 μs** | **251.2 μs** | **4,055.1** |
|  OneTemplateLog |           Json |  False |     True | 242.8 μs | 100.93 μs | 5.53 μs | 3.19 μs | 236.4 μs | 241.0 μs | 245.5 μs | 246.0 μs | 246.5 μs | 4,118.6 |
| FiveTemplateLog |           Json |  False |     True | 262.8 μs |  52.81 μs | 2.89 μs | 1.67 μs | 260.9 μs | 261.1 μs | 261.3 μs | 263.7 μs | 266.1 μs | 3,805.6 |
|       **SimpleLog** |           **Json** |   **True** |    **False** | **204.5 μs** |  **33.57 μs** | **1.84 μs** | **1.06 μs** | **202.4 μs** | **203.8 μs** | **205.2 μs** | **205.5 μs** | **205.9 μs** | **4,890.3** |
|  OneTemplateLog |           Json |   True |    False | 205.3 μs |  38.01 μs | 2.08 μs | 1.20 μs | 203.5 μs | 204.2 μs | 204.9 μs | 206.2 μs | 207.6 μs | 4,870.4 |
| FiveTemplateLog |           Json |   True |    False | 249.5 μs |  19.42 μs | 1.06 μs | 0.61 μs | 248.5 μs | 248.9 μs | 249.4 μs | 250.0 μs | 250.6 μs | 4,008.2 |
|       **SimpleLog** |           **Json** |   **True** |     **True** | **298.3 μs** |  **21.90 μs** | **1.20 μs** | **0.69 μs** | **297.2 μs** | **297.7 μs** | **298.1 μs** | **298.8 μs** | **299.6 μs** | **3,352.5** |
|  OneTemplateLog |           Json |   True |     True | 302.5 μs |  15.21 μs | 0.83 μs | 0.48 μs | 301.9 μs | 302.0 μs | 302.2 μs | 302.8 μs | 303.5 μs | 3,305.6 |
| FiveTemplateLog |           Json |   True |     True | 311.3 μs |  59.73 μs | 3.27 μs | 1.89 μs | 308.2 μs | 309.6 μs | 310.9 μs | 312.8 μs | 314.7 μs | 3,212.4 |
|       **SimpleLog** |         **MyJson** |  **False** |    **False** | **119.7 μs** |  **14.76 μs** | **0.81 μs** | **0.47 μs** | **118.8 μs** | **119.4 μs** | **120.0 μs** | **120.2 μs** | **120.4 μs** | **8,351.5** |
|  OneTemplateLog |         MyJson |  False |    False | 159.5 μs |   9.40 μs | 0.52 μs | 0.30 μs | 159.0 μs | 159.2 μs | 159.4 μs | 159.7 μs | 160.0 μs | 6,270.0 |
| FiveTemplateLog |         MyJson |  False |    False | 161.2 μs |  14.42 μs | 0.79 μs | 0.46 μs | 160.5 μs | 160.8 μs | 161.1 μs | 161.6 μs | 162.0 μs | 6,203.9 |
|       **SimpleLog** |         **MyJson** |  **False** |     **True** | **178.4 μs** |  **18.58 μs** | **1.02 μs** | **0.59 μs** | **177.2 μs** | **178.1 μs** | **178.9 μs** | **179.0 μs** | **179.1 μs** | **5,605.1** |
|  OneTemplateLog |         MyJson |  False |     True | 198.6 μs |  26.05 μs | 1.43 μs | 0.82 μs | 197.2 μs | 197.9 μs | 198.6 μs | 199.3 μs | 200.1 μs | 5,034.3 |
| FiveTemplateLog |         MyJson |  False |     True | 199.7 μs |  22.21 μs | 1.22 μs | 0.70 μs | 198.7 μs | 199.0 μs | 199.3 μs | 200.2 μs | 201.0 μs | 5,007.8 |
|       **SimpleLog** |         **MyJson** |   **True** |    **False** | **156.4 μs** |  **22.52 μs** | **1.23 μs** | **0.71 μs** | **155.6 μs** | **155.7 μs** | **155.7 μs** | **156.7 μs** | **157.8 μs** | **6,394.8** |
|  OneTemplateLog |         MyJson |   True |    False | 171.3 μs |  32.62 μs | 1.79 μs | 1.03 μs | 169.6 μs | 170.3 μs | 171.0 μs | 172.1 μs | 173.2 μs | 5,838.2 |
| FiveTemplateLog |         MyJson |   True |    False | 173.9 μs |  14.64 μs | 0.80 μs | 0.46 μs | 173.3 μs | 173.4 μs | 173.6 μs | 174.2 μs | 174.8 μs | 5,750.5 |
|       **SimpleLog** |         **MyJson** |   **True** |     **True** | **195.8 μs** |  **29.33 μs** | **1.61 μs** | **0.93 μs** | **194.5 μs** | **194.9 μs** | **195.3 μs** | **196.4 μs** | **197.6 μs** | **5,108.2** |
|  OneTemplateLog |         MyJson |   True |     True | 208.0 μs |  39.20 μs | 2.15 μs | 1.24 μs | 205.7 μs | 207.1 μs | 208.6 μs | 209.2 μs | 209.9 μs | 4,807.2 |
| FiveTemplateLog |         MyJson |   True |     True | 222.7 μs |   5.76 μs | 0.32 μs | 0.18 μs | 222.4 μs | 222.6 μs | 222.7 μs | 222.9 μs | 223.1 μs | 4,489.7 |


## Legenda

```
// * Warnings *
Environment
  Summary -> Benchmark was executed with attached debugger

// * Legends *
  LoggerProvider : Value of the 'LoggerProvider' parameter
  Scopes         : Value of the 'Scopes' parameter
  Activity       : Value of the 'Activity' parameter
  Mean           : Arithmetic mean of all measurements
  Error          : Half of 99.9% confidence interval
  StdDev         : Standard deviation of all measurements
  StdErr         : Standard error of all measurements
  Min            : Minimum
  Q1             : Quartile 1 (25th percentile)
  Median         : Value separating the higher half of all measurements (50th percentile)
  Q3             : Quartile 3 (75th percentile)
  Max            : Maximum
  Op/s           : Operation per second
  1 μs           : 1 Microsecond (0.000001 sec)
```
