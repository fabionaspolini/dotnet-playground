# microsoft-logging-benchmark-playground


## MyJsonConsole -> SimpleLog (ShortRunJob) -> ActivityTrackingOptions: SpanId, TraceId, ParentId, Tags, Baggage

|    Method | LoggerProvider | Scopes | Activity |     Mean |    Error |  StdDev |  StdErr |      Min |       Q1 |   Median |       Q3 |      Max |    Op/s |
|---------- |--------------- |------- |--------- |---------:|---------:|--------:|--------:|---------:|---------:|---------:|---------:|---------:|--------:|
| SimpleLog |         MyJson |  False |    False | 122.1 μs | 38.77 μs | 2.13 μs | 1.23 μs | 120.1 μs | 120.9 μs | 121.7 μs | 123.0 μs | 124.3 μs | 8,193.0 |
| SimpleLog |         MyJson |  False |     True | 179.9 μs | 39.69 μs | 2.18 μs | 1.26 μs | 178.6 μs | 178.6 μs | 178.7 μs | 180.5 μs | 182.4 μs | 5,559.3 |
| SimpleLog |         MyJson |   True |    False | 156.6 μs | 19.74 μs | 1.08 μs | 0.62 μs | 156.0 μs | 156.0 μs | 156.1 μs | 157.0 μs | 157.9 μs | 6,383.9 |
| SimpleLog |         MyJson |   True |     True | 198.5 μs | 11.68 μs | 0.64 μs | 0.37 μs | 198.1 μs | 198.1 μs | 198.1 μs | 198.7 μs | 199.2 μs | 5,038.7 |

## SimpleConsole -> All tests (ShortRunJob) -> ActivityTrackingOptions: SpanId, TraceId, ParentId, Tags, Baggage

|          Method | LoggerProvider | Scopes | Activity |     Mean |     Error |   StdDev |  StdErr |      Min |       Q1 |   Median |       Q3 |      Max |    Op/s |
|---------------- |--------------- |------- |--------- |---------:|----------:|---------:|--------:|---------:|---------:|---------:|---------:|---------:|--------:|
|       **SimpleLog** |  **SimpleConsole** |  **False** |    **False** | **100.6 μs** |   **4.97 μs** |  **0.27 μs** | **0.16 μs** | **100.4 μs** | **100.5 μs** | **100.6 μs** | **100.7 μs** | **100.9 μs** | **9,938.0** |
|  OneTemplateLog |  SimpleConsole |  False |    False | 101.1 μs |   5.54 μs |  0.30 μs | 0.18 μs | 100.9 μs | 100.9 μs | 101.0 μs | 101.2 μs | 101.4 μs | 9,890.0 |
| FiveTemplateLog |  SimpleConsole |  False |    False | 103.1 μs |  35.44 μs |  1.94 μs | 1.12 μs | 100.9 μs | 102.5 μs | 104.1 μs | 104.2 μs | 104.4 μs | 9,697.4 |
|       **SimpleLog** |  **SimpleConsole** |  **False** |     **True** | **136.9 μs** |  **21.19 μs** |  **1.16 μs** | **0.67 μs** | **135.5 μs** | **136.4 μs** | **137.3 μs** | **137.5 μs** | **137.7 μs** | **7,306.6** |
|  OneTemplateLog |  SimpleConsole |  False |     True | 137.8 μs |  45.35 μs |  2.49 μs | 1.44 μs | 136.1 μs | 136.3 μs | 136.6 μs | 138.6 μs | 140.6 μs | 7,259.4 |
| FiveTemplateLog |  SimpleConsole |  False |     True | 136.7 μs |   7.89 μs |  0.43 μs | 0.25 μs | 136.2 μs | 136.6 μs | 136.9 μs | 137.0 μs | 137.0 μs | 7,314.0 |
|       **SimpleLog** |  **SimpleConsole** |   **True** |    **False** | **123.7 μs** |  **18.98 μs** |  **1.04 μs** | **0.60 μs** | **122.6 μs** | **123.2 μs** | **123.8 μs** | **124.2 μs** | **124.7 μs** | **8,084.6** |
|  OneTemplateLog |  SimpleConsole |   True |    False | 127.2 μs |  16.88 μs |  0.92 μs | 0.53 μs | 126.4 μs | 126.6 μs | 126.9 μs | 127.5 μs | 128.2 μs | 7,864.4 |
| FiveTemplateLog |  SimpleConsole |   True |    False | 126.5 μs |   9.11 μs |  0.50 μs | 0.29 μs | 126.1 μs | 126.2 μs | 126.4 μs | 126.7 μs | 127.0 μs | 7,905.7 |
|       **SimpleLog** |  **SimpleConsole** |   **True** |     **True** | **170.2 μs** |  **49.45 μs** |  **2.71 μs** | **1.56 μs** | **168.1 μs** | **168.7 μs** | **169.2 μs** | **171.2 μs** | **173.3 μs** | **5,875.6** |
|  OneTemplateLog |  SimpleConsole |   True |     True | 171.7 μs |  11.57 μs |  0.63 μs | 0.37 μs | 171.0 μs | 171.4 μs | 171.8 μs | 172.0 μs | 172.3 μs | 5,825.1 |
| FiveTemplateLog |  SimpleConsole |   True |     True | 179.1 μs | 192.64 μs | 10.56 μs | 6.10 μs | 172.4 μs | 173.0 μs | 173.6 μs | 182.4 μs | 191.3 μs | 5,583.9 |




## MyJson -> All tests (ShortRunJob) -> ActivityTrackingOptions: SpanId, TraceId, ParentId, Tags, Baggage

Este é cenário esperado para executar por padrão na aplicação.

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

## MyJson -> All tests (ShortRunJob) -> ActivityTrackingOptions: None

Teste desabilitando o recurso que adiciona informações da activity ao log.  
O resultado efetivo de todos os casos de testes com `Activity=true` é um log sem os dados adicionais das tags e baggages.

|          Method | LoggerProvider | Scopes | Activity |     Mean |    Error |  StdDev |  StdErr |      Min |       Q1 |   Median |       Q3 |      Max |    Op/s |
|---------------- |--------------- |------- |--------- |---------:|---------:|--------:|--------:|---------:|---------:|---------:|---------:|---------:|--------:|
|       **SimpleLog** |         **MyJson** |  **False** |    **False** | **120.1 μs** | **24.49 μs** | **1.34 μs** | **0.78 μs** | **118.6 μs** | **119.5 μs** | **120.4 μs** | **120.8 μs** | **121.2 μs** | **8,326.4** |
|  OneTemplateLog |         MyJson |  False |    False | 158.8 μs |  9.22 μs | 0.51 μs | 0.29 μs | 158.4 μs | 158.5 μs | 158.6 μs | 159.0 μs | 159.3 μs | 6,298.4 |
| FiveTemplateLog |         MyJson |  False |    False | 164.4 μs | 81.26 μs | 4.45 μs | 2.57 μs | 160.8 μs | 161.9 μs | 163.1 μs | 166.2 μs | 169.4 μs | 6,081.4 |
|       **SimpleLog** |         **MyJson** |  **False** |     **True** | **138.8 μs** | **60.08 μs** | **3.29 μs** | **1.90 μs** | **135.7 μs** | **137.1 μs** | **138.4 μs** | **140.3 μs** | **142.3 μs** | **7,205.1** |
|  OneTemplateLog |         MyJson |  False |     True | 166.6 μs | 34.06 μs | 1.87 μs | 1.08 μs | 164.8 μs | 165.7 μs | 166.6 μs | 167.5 μs | 168.5 μs | 6,001.7 |
| FiveTemplateLog |         MyJson |  False |     True | 170.4 μs | 53.81 μs | 2.95 μs | 1.70 μs | 167.2 μs | 169.1 μs | 170.9 μs | 172.0 μs | 173.0 μs | 5,869.3 |
|       **SimpleLog** |         **MyJson** |   **True** |    **False** | **159.1 μs** | **45.43 μs** | **2.49 μs** | **1.44 μs** | **156.9 μs** | **157.8 μs** | **158.7 μs** | **160.3 μs** | **161.8 μs** | **6,283.6** |
|  OneTemplateLog |         MyJson |   True |    False | 172.1 μs | 46.35 μs | 2.54 μs | 1.47 μs | 169.2 μs | 171.3 μs | 173.5 μs | 173.6 μs | 173.6 μs | 5,810.9 |
| FiveTemplateLog |         MyJson |   True |    False | 174.1 μs | 22.59 μs | 1.24 μs | 0.72 μs | 172.8 μs | 173.6 μs | 174.4 μs | 174.8 μs | 175.2 μs | 5,743.0 |
|       **SimpleLog** |         **MyJson** |   **True** |     **True** | **164.5 μs** | **45.94 μs** | **2.52 μs** | **1.45 μs** | **162.7 μs** | **163.1 μs** | **163.6 μs** | **165.5 μs** | **167.4 μs** | **6,077.6** |
|  OneTemplateLog |         MyJson |   True |     True | 177.7 μs | 67.91 μs | 3.72 μs | 2.15 μs | 175.3 μs | 175.6 μs | 175.8 μs | 178.9 μs | 182.0 μs | 5,627.4 |
| FiveTemplateLog |         MyJson |   True |     True | 178.5 μs | 57.63 μs | 3.16 μs | 1.82 μs | 176.5 μs | 176.7 μs | 176.9 μs | 179.5 μs | 182.1 μs | 5,602.0 |

## MyJson -> All tests (ShortRunJob) -> ActivityTrackingOptions: SpanId, TraceId, ParentId, Tags, Baggage + LogLevel: Warning

Teste completo da feature, porem adicionando filtro de log level para não imprimir no console nenhum log information.

|          Method | LoggerProvider | Scopes | Activity |      Mean |      Error |    StdDev |   StdErr |       Min |        Q1 |    Median |        Q3 |       Max |         Op/s |
|---------------- |--------------- |------- |--------- |----------:|-----------:|----------:|---------:|----------:|----------:|----------:|----------:|----------:|-------------:|
|       **SimpleLog** |         **MyJson** |  **False** |    **False** |  **22.03 ns** |   **3.812 ns** |  **0.209 ns** | **0.121 ns** |  **21.86 ns** |  **21.92 ns** |  **21.97 ns** |  **22.12 ns** |  **22.27 ns** | **45,387,865.5** |
|  OneTemplateLog |         MyJson |  False |    False |  48.47 ns |   3.620 ns |  0.198 ns | 0.115 ns |  48.29 ns |  48.37 ns |  48.45 ns |  48.57 ns |  48.68 ns | 20,629,465.8 |
| FiveTemplateLog |         MyJson |  False |    False |  89.76 ns |   8.277 ns |  0.454 ns | 0.262 ns |  89.24 ns |  89.63 ns |  90.02 ns |  90.03 ns |  90.03 ns | 11,140,312.3 |
|       **SimpleLog** |         **MyJson** |  **False** |     **True** | **264.76 ns** |  **97.824 ns** |  **5.362 ns** | **3.096 ns** | **258.58 ns** | **263.01 ns** | **267.43 ns** | **267.84 ns** | **268.25 ns** |  **3,777,045.9** |
|  OneTemplateLog |         MyJson |  False |     True | 280.97 ns | 191.724 ns | 10.509 ns | 6.067 ns | 268.92 ns | 277.32 ns | 285.72 ns | 286.99 ns | 288.26 ns |  3,559,091.6 |
| FiveTemplateLog |         MyJson |  False |     True | 323.13 ns |  31.414 ns |  1.722 ns | 0.994 ns | 321.35 ns | 322.31 ns | 323.26 ns | 324.03 ns | 324.79 ns |  3,094,687.2 |
|       **SimpleLog** |         **MyJson** |   **True** |    **False** |  **78.45 ns** |  **51.805 ns** |  **2.840 ns** | **1.639 ns** |  **76.24 ns** |  **76.85 ns** |  **77.47 ns** |  **79.56 ns** |  **81.65 ns** | **12,746,458.6** |
|  OneTemplateLog |         MyJson |   True |    False | 100.32 ns |  11.255 ns |  0.617 ns | 0.356 ns |  99.69 ns | 100.02 ns | 100.34 ns | 100.63 ns | 100.92 ns |  9,968,312.0 |
| FiveTemplateLog |         MyJson |   True |    False | 148.32 ns |  40.779 ns |  2.235 ns | 1.291 ns | 145.74 ns | 147.64 ns | 149.53 ns | 149.61 ns | 149.68 ns |  6,742,245.2 |
|       **SimpleLog** |         **MyJson** |   **True** |     **True** | **381.42 ns** | **159.158 ns** |  **8.724 ns** | **5.037 ns** | **371.39 ns** | **378.50 ns** | **385.61 ns** | **386.43 ns** | **387.25 ns** |  **2,621,794.3** |
|  OneTemplateLog |         MyJson |   True |     True | 389.56 ns | 163.180 ns |  8.944 ns | 5.164 ns | 379.66 ns | 385.81 ns | 391.95 ns | 394.51 ns | 397.06 ns |  2,567,003.1 |
| FiveTemplateLog |         MyJson |   True |     True | 436.59 ns | 107.971 ns |  5.918 ns | 3.417 ns | 429.78 ns | 434.64 ns | 439.51 ns | 440.00 ns | 440.49 ns |  2,290,465.5 |

## SimpleConsole -> All tests (ShortRunJob) -> ActivityTrackingOptions: SpanId, TraceId, ParentId, Tags, Baggage + LogLevel: Warning

Teste completo da feature, porem adicionando filtro de log level para não imprimir no console nenhum log information.

|          Method | LoggerProvider | Scopes | Activity |      Mean |      Error |    StdDev |   StdErr |       Min |        Q1 |    Median |        Q3 |       Max |         Op/s |
|---------------- |--------------- |------- |--------- |----------:|-----------:|----------:|---------:|----------:|----------:|----------:|----------:|----------:|-------------:|
|       **SimpleLog** |  **SimpleConsole** |  **False** |    **False** |  **21.67 ns** |   **1.309 ns** |  **0.072 ns** | **0.041 ns** |  **21.61 ns** |  **21.63 ns** |  **21.66 ns** |  **21.70 ns** |  **21.75 ns** | **46,140,352.9** |
|  OneTemplateLog |  SimpleConsole |  False |    False |  44.87 ns |   7.246 ns |  0.397 ns | 0.229 ns |  44.55 ns |  44.64 ns |  44.73 ns |  45.02 ns |  45.31 ns | 22,288,982.8 |
| FiveTemplateLog |  SimpleConsole |  False |    False |  87.84 ns |   4.120 ns |  0.226 ns | 0.130 ns |  87.64 ns |  87.72 ns |  87.81 ns |  87.95 ns |  88.08 ns | 11,383,820.3 |
|       **SimpleLog** |  **SimpleConsole** |  **False** |     **True** | **259.37 ns** | **101.627 ns** |  **5.571 ns** | **3.216 ns** | **252.94 ns** | **257.69 ns** | **262.44 ns** | **262.58 ns** | **262.72 ns** |  **3,855,532.0** |
|  OneTemplateLog |  SimpleConsole |  False |     True | 292.96 ns | 123.003 ns |  6.742 ns | 3.893 ns | 286.07 ns | 289.67 ns | 293.27 ns | 296.41 ns | 299.55 ns |  3,413,391.5 |
| FiveTemplateLog |  SimpleConsole |  False |     True | 340.00 ns | 185.092 ns | 10.146 ns | 5.858 ns | 328.68 ns | 335.86 ns | 343.04 ns | 345.66 ns | 348.27 ns |  2,941,212.7 |
|       **SimpleLog** |  **SimpleConsole** |   **True** |    **False** |  **79.43 ns** |  **14.113 ns** |  **0.774 ns** | **0.447 ns** |  **78.55 ns** |  **79.15 ns** |  **79.76 ns** |  **79.87 ns** |  **79.99 ns** | **12,589,428.3** |
|  OneTemplateLog |  SimpleConsole |   True |    False | 102.49 ns |  11.290 ns |  0.619 ns | 0.357 ns | 101.85 ns | 102.20 ns | 102.54 ns | 102.81 ns | 103.08 ns |  9,756,966.6 |
| FiveTemplateLog |  SimpleConsole |   True |    False | 150.96 ns |  24.709 ns |  1.354 ns | 0.782 ns | 149.52 ns | 150.33 ns | 151.14 ns | 151.68 ns | 152.21 ns |  6,624,294.0 |
|       **SimpleLog** |  **SimpleConsole** |   **True** |     **True** | **347.33 ns** | **132.539 ns** |  **7.265 ns** | **4.194 ns** | **339.00 ns** | **344.81 ns** | **350.62 ns** | **351.49 ns** | **352.36 ns** |  **2,879,144.6** |
|  OneTemplateLog |  SimpleConsole |   True |     True | 416.55 ns | 162.944 ns |  8.932 ns | 5.157 ns | 406.33 ns | 413.39 ns | 420.44 ns | 421.66 ns | 422.87 ns |  2,400,682.0 |
| FiveTemplateLog |  SimpleConsole |   True |     True | 480.77 ns | 151.594 ns |  8.309 ns | 4.797 ns | 472.19 ns | 476.76 ns | 481.32 ns | 485.05 ns | 488.78 ns |  2,080,016.5 |



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
