# Collections

- [TL;DR;](#tldr)
- [String](#string)
  - [ConcurrentDictionary\<string, integer\>](#concurrentdictionarystring-integer)
  - [Dictionary\<string, int\>](#dictionarystring-int)
  - [HashSet\<string\>](#hashsetstring)
  - [HashTable (string)](#hashtable-string)
  - [LinkedList\<string\>](#linkedliststring)
  - [List\<string\>](#liststring)
  - [SortedList\<string, integer\>](#sortedliststring-integer)
- [Integer](#integer)
  - [HashSet\<integer\>](#hashsetinteger)
- [Legends](#legends)

Análise detalhada: <https://tutorials.eu/c-sharp-collections-performance/#:~:text=The%20performance%20of%20C%23%20collections,be%20added%20or%20removed%20frequently>

## TL;DR;

| Classe                                | Escolher quando |
|---------------------------------------|-----------------|
| ConcurrentDictionary\<TKey, TValue\>  | Elementos únicos que precisam ser acessados com frequência, e de forma concorrente entre threads  |
| Dictionary\<TKey, TValue\>            | Elementos únicos que precisam ser acessados com frequência |
| HashSet\<T\>                          | Elementos únicos que precisam ser adicionados, recuperados ou pesquisados ​​com frequência |
| LinkedList\<T\>                       | Inserção ou exclusão rápida de elementos no meio da lista |
| List\<T\>                             | Inserção, exclusão e acesso por index rápida, mas a pesquisa é lenta  |
| SortedList\<TKey, TValue\>            | Elementos únicos com inserção rápida, mas a pesquisa pode ser lenta |

## String

### ConcurrentDictionary\<string, integer\>

| Method      | FindValue     | Mean      | Error | StdErr   | StdDev   | Min       | Q1        | Median    | Q3        | Max       | Op/s          | Allocated |
|------------ |-------------- |----------:|------:|---------:|---------:|----------:|----------:|----------:|----------:|----------:|--------------:|----------:|
| TryGetValue | Teste 1       |  9.196 ns |    NA | 0.000 ns | 0.000 ns |  9.196 ns |  9.196 ns |  9.196 ns |  9.196 ns |  9.196 ns | 108,737,806.7 |         - |
| TryGetValue | Teste 568452  |  9.747 ns |    NA | 0.000 ns | 0.000 ns |  9.747 ns |  9.747 ns |  9.747 ns |  9.747 ns |  9.747 ns | 102,599,638.5 |         - |
| TryGetValue | Teste 1000000 |  9.775 ns |    NA | 0.000 ns | 0.000 ns |  9.775 ns |  9.775 ns |  9.775 ns |  9.775 ns |  9.775 ns | 102,306,023.8 |         - |
|             |               |           |       |          |          |           |           |           |           |           |               |           |
| Contains    | Teste 1       |  9.136 ns |    NA | 0.000 ns | 0.000 ns |  9.136 ns |  9.136 ns |  9.136 ns |  9.136 ns |  9.136 ns | 109,460,785.2 |         - |
| Contains    | Teste 568452  |  9.506 ns |    NA | 0.000 ns | 0.000 ns |  9.506 ns |  9.506 ns |  9.506 ns |  9.506 ns |  9.506 ns | 105,201,510.3 |         - |
| Contains    | Teste 1000000 |  9.745 ns |    NA | 0.000 ns | 0.000 ns |  9.745 ns |  9.745 ns |  9.745 ns |  9.745 ns |  9.745 ns | 102,620,206.9 |         - |
|             |               |           |       |          |          |           |           |           |           |           |               |           |
| IndexValue  | Teste 1       | 10.016 ns |    NA | 0.000 ns | 0.000 ns | 10.016 ns | 10.016 ns | 10.016 ns | 10.016 ns | 10.016 ns |  99,838,871.5 |         - |
| IndexValue  | Teste 1000000 | 10.381 ns |    NA | 0.000 ns | 0.000 ns | 10.381 ns | 10.381 ns | 10.381 ns | 10.381 ns | 10.381 ns |  96,331,661.0 |         - |
| IndexValue  | Teste 568452  | 10.422 ns |    NA | 0.000 ns | 0.000 ns | 10.422 ns | 10.422 ns | 10.422 ns | 10.422 ns | 10.422 ns |  95,951,775.9 |         - |

### Dictionary\<string, int\>

| Method      | FindValue     | Mean      | Error | StdErr   | StdDev   | Min       | Q1        | Median    | Q3        | Max       | Op/s          | Allocated |
|------------ |-------------- |----------:|------:|---------:|---------:|----------:|----------:|----------:|----------:|----------:|--------------:|----------:|
| TryGetValue | Teste 568452  |  9.887 ns |    NA | 0.000 ns | 0.000 ns |  9.887 ns |  9.887 ns |  9.887 ns |  9.887 ns |  9.887 ns | 101,145,832.4 |         - |
| TryGetValue | Teste 1       |  9.947 ns |    NA | 0.000 ns | 0.000 ns |  9.947 ns |  9.947 ns |  9.947 ns |  9.947 ns |  9.947 ns | 100,529,702.1 |         - |
| TryGetValue | Teste 1000000 | 10.276 ns |    NA | 0.000 ns | 0.000 ns | 10.276 ns | 10.276 ns | 10.276 ns | 10.276 ns | 10.276 ns |  97,316,922.3 |         - |
|             |               |           |       |          |          |           |           |           |           |           |               |           |
| Contains    | Teste 568452  |  9.903 ns |    NA | 0.000 ns | 0.000 ns |  9.903 ns |  9.903 ns |  9.903 ns |  9.903 ns |  9.903 ns | 100,982,066.1 |         - |
| Contains    | Teste 1000000 |  9.972 ns |    NA | 0.000 ns | 0.000 ns |  9.972 ns |  9.972 ns |  9.972 ns |  9.972 ns |  9.972 ns | 100,284,156.7 |         - |
| Contains    | Teste 1       | 11.153 ns |    NA | 0.000 ns | 0.000 ns | 11.153 ns | 11.153 ns | 11.153 ns | 11.153 ns | 11.153 ns |  89,661,334.7 |         - |
|             |               |           |       |          |          |           |           |           |           |           |               |           |
| IndexValue  | Teste 1       | 10.566 ns |    NA | 0.000 ns | 0.000 ns | 10.566 ns | 10.566 ns | 10.566 ns | 10.566 ns | 10.566 ns |  94,645,658.8 |         - |
| IndexValue  | Teste 568452  | 10.749 ns |    NA | 0.000 ns | 0.000 ns | 10.749 ns | 10.749 ns | 10.749 ns | 10.749 ns | 10.749 ns |  93,028,592.9 |         - |
| IndexValue  | Teste 1000000 | 10.809 ns |    NA | 0.000 ns | 0.000 ns | 10.809 ns | 10.809 ns | 10.809 ns | 10.809 ns | 10.809 ns |  92,514,630.5 |         - |

### HashSet\<string\>

| Method         | FindValue     | Mean             | Error | StdErr   | StdDev   | Min              | Q1               | Median           | Q3               | Max              | Op/s          | Gen0   | Allocated |
|--------------- |-------------- |-----------------:|------:|---------:|---------:|-----------------:|-----------------:|-----------------:|-----------------:|-----------------:|--------------:|-------:|----------:|
| TryGetValue    | Teste 1000000 |         7.785 ns |    NA | 0.000 ns | 0.000 ns |         7.785 ns |         7.785 ns |         7.785 ns |         7.785 ns |         7.785 ns | 128,448,489.8 |      - |         - |
| TryGetValue    | Teste 1       |        10.485 ns |    NA | 0.000 ns | 0.000 ns |        10.485 ns |        10.485 ns |        10.485 ns |        10.485 ns |        10.485 ns |  95,375,772.8 |      - |         - |
| TryGetValue    | Teste 568452  |        10.852 ns |    NA | 0.000 ns | 0.000 ns |        10.852 ns |        10.852 ns |        10.852 ns |        10.852 ns |        10.852 ns |  92,149,354.5 |      - |         - |
|                |               |                  |       |          |          |                  |                  |                  |                  |                  |               |        |           |
| Contains       | Teste 1000000 |         7.110 ns |    NA | 0.000 ns | 0.000 ns |         7.110 ns |         7.110 ns |         7.110 ns |         7.110 ns |         7.110 ns | 140,638,169.9 |      - |         - |
| Contains       | Teste 1       |         9.763 ns |    NA | 0.000 ns | 0.000 ns |         9.763 ns |         9.763 ns |         9.763 ns |         9.763 ns |         9.763 ns | 102,422,473.7 |      - |         - |
| Contains       | Teste 568452  |         9.831 ns |    NA | 0.000 ns | 0.000 ns |         9.831 ns |         9.831 ns |         9.831 ns |         9.831 ns |         9.831 ns | 101,714,633.7 |      - |         - |
|                |               |                  |       |          |          |                  |                  |                  |                  |                  |               |        |           |
| FirstOrDefault | Teste 1       |        24.753 ns |    NA | 0.000 ns | 0.000 ns |        24.753 ns |        24.753 ns |        24.753 ns |        24.753 ns |        24.753 ns |  40,399,749.4 | 0.0124 |     104 B |
| FirstOrDefault | Teste 568452  | 4,005,480.469 ns |    NA | 0.000 ns | 0.000 ns | 4,005,480.469 ns | 4,005,480.469 ns | 4,005,480.469 ns | 4,005,480.469 ns | 4,005,480.469 ns |         249.7 |      - |     110 B |
| FirstOrDefault | Teste 1000000 | 4,826,250.000 ns |    NA | 0.000 ns | 0.000 ns | 4,826,250.000 ns | 4,826,250.000 ns | 4,826,250.000 ns | 4,826,250.000 ns | 4,826,250.000 ns |         207.2 |      - |     116 B |

### HashTable (string)

| Method     | FindValue     | Mean     | Error | StdErr  | StdDev  | Min      | Q1       | Median   | Q3       | Max      | Op/s         | Allocated |
|----------- |-------------- |---------:|------:|--------:|--------:|---------:|---------:|---------:|---------:|---------:|-------------:|----------:|
| Contains   | Teste 1       | 11.03 ns |    NA | 0.00 ns | 0.00 ns | 11.03 ns | 11.03 ns | 11.03 ns | 11.03 ns | 11.03 ns | 90,663,407.7 |         - |
| Contains   | Teste 568452  | 14.16 ns |    NA | 0.00 ns | 0.00 ns | 14.16 ns | 14.16 ns | 14.16 ns | 14.16 ns | 14.16 ns | 70,628,999.2 |         - |
| Contains   | Teste 1000000 | 17.13 ns |    NA | 0.00 ns | 0.00 ns | 17.13 ns | 17.13 ns | 17.13 ns | 17.13 ns | 17.13 ns | 58,387,489.6 |         - |
|            |               |          |       |         |         |          |          |          |          |          |              |           |
| IndexValue | Teste 1       | 11.22 ns |    NA | 0.00 ns | 0.00 ns | 11.22 ns | 11.22 ns | 11.22 ns | 11.22 ns | 11.22 ns | 89,146,030.1 |         - |
| IndexValue | Teste 568452  | 17.88 ns |    NA | 0.00 ns | 0.00 ns | 17.88 ns | 17.88 ns | 17.88 ns | 17.88 ns | 17.88 ns | 55,938,606.7 |         - |
| IndexValue | Teste 1000000 | 55.99 ns |    NA | 0.00 ns | 0.00 ns | 55.99 ns | 55.99 ns | 55.99 ns | 55.99 ns | 55.99 ns | 17,858,853.5 |         - |

### LinkedList\<string\>

| Method         | FindValue     | Mean             | Error | StdErr   | StdDev   | Min              | Q1               | Median           | Q3               | Max              | Op/s          | Gen0   | Allocated |
|--------------- |-------------- |-----------------:|------:|---------:|---------:|-----------------:|-----------------:|-----------------:|-----------------:|-----------------:|--------------:|-------:|----------:|
| Find           | Teste 1       |         7.320 ns |    NA | 0.000 ns | 0.000 ns |         7.320 ns |         7.320 ns |         7.320 ns |         7.320 ns |         7.320 ns | 136,609,486.9 |      - |         - |
| Find           | Teste 568452  | 3,239,637.500 ns |    NA | 0.000 ns | 0.000 ns | 3,239,637.500 ns | 3,239,637.500 ns | 3,239,637.500 ns | 3,239,637.500 ns | 3,239,637.500 ns |         308.7 |      - |       2 B |
| Find           | Teste 1000000 | 5,402,802.734 ns |    NA | 0.000 ns | 0.000 ns | 5,402,802.734 ns | 5,402,802.734 ns | 5,402,802.734 ns | 5,402,802.734 ns | 5,402,802.734 ns |         185.1 |      - |       3 B |
|                |               |                  |       |          |          |                  |                  |                  |                  |                  |               |        |           |
| Contains       | Teste 1       |         7.652 ns |    NA | 0.000 ns | 0.000 ns |         7.652 ns |         7.652 ns |         7.652 ns |         7.652 ns |         7.652 ns | 130,679,476.6 |      - |         - |
| Contains       | Teste 568452  | 2,926,298.047 ns |    NA | 0.000 ns | 0.000 ns | 2,926,298.047 ns | 2,926,298.047 ns | 2,926,298.047 ns | 2,926,298.047 ns | 2,926,298.047 ns |         341.7 |      - |       2 B |
| Contains       | Teste 1000000 | 5,469,114.453 ns |    NA | 0.000 ns | 0.000 ns | 5,469,114.453 ns | 5,469,114.453 ns | 5,469,114.453 ns | 5,469,114.453 ns | 5,469,114.453 ns |         182.8 |      - |       3 B |
|                |               |                  |       |          |          |                  |                  |                  |                  |                  |               |        |           |
| FirstOrDefault | Teste 1       |        20.386 ns |    NA | 0.000 ns | 0.000 ns |        20.386 ns |        20.386 ns |        20.386 ns |        20.386 ns |        20.386 ns |  49,052,536.9 | 0.0134 |     112 B |
| FirstOrDefault | Teste 568452  | 5,171,405.469 ns |    NA | 0.000 ns | 0.000 ns | 5,171,405.469 ns | 5,171,405.469 ns | 5,171,405.469 ns | 5,171,405.469 ns | 5,171,405.469 ns |         193.4 |      - |     118 B |
| FirstOrDefault | Teste 1000000 | 7,501,734.375 ns |    NA | 0.000 ns | 0.000 ns | 7,501,734.375 ns | 7,501,734.375 ns | 7,501,734.375 ns | 7,501,734.375 ns | 7,501,734.375 ns |         133.3 |      - |     124 B |

### List\<string\>

| Method         | FindValue     | Mean             | Error | StdErr   | StdDev   | Min              | Q1               | Median           | Q3               | Max              | Op/s          | Gen0   | Allocated |
|--------------- |-------------- |-----------------:|------:|---------:|---------:|-----------------:|-----------------:|-----------------:|-----------------:|-----------------:|--------------:|-------:|----------:|
| Find           | Teste 1       |         8.426 ns |    NA | 0.000 ns | 0.000 ns |         8.426 ns |         8.426 ns |         8.426 ns |         8.426 ns |         8.426 ns | 118,681,742.0 | 0.0076 |      64 B |
| Find           | Teste 568452  | 1,867,599.512 ns |    NA | 0.000 ns | 0.000 ns | 1,867,599.512 ns | 1,867,599.512 ns | 1,867,599.512 ns | 1,867,599.512 ns | 1,867,599.512 ns |         535.4 |      - |      65 B |
| Find           | Teste 1000000 | 2,395,780.859 ns |    NA | 0.000 ns | 0.000 ns | 2,395,780.859 ns | 2,395,780.859 ns | 2,395,780.859 ns | 2,395,780.859 ns | 2,395,780.859 ns |         417.4 |      - |      66 B |
|                |               |                  |       |          |          |                  |                  |                  |                  |                  |               |        |           |
| Contains       | Teste 1       |         8.888 ns |    NA | 0.000 ns | 0.000 ns |         8.888 ns |         8.888 ns |         8.888 ns |         8.888 ns |         8.888 ns | 112,514,398.2 |      - |         - |
| Contains       | Teste 568452  | 2,194,412.305 ns |    NA | 0.000 ns | 0.000 ns | 2,194,412.305 ns | 2,194,412.305 ns | 2,194,412.305 ns | 2,194,412.305 ns | 2,194,412.305 ns |         455.7 |      - |       2 B |
| Contains       | Teste 1000000 | 2,521,701.562 ns |    NA | 0.000 ns | 0.000 ns | 2,521,701.562 ns | 2,521,701.562 ns | 2,521,701.562 ns | 2,521,701.562 ns | 2,521,701.562 ns |         396.6 |      - |       2 B |
|                |               |                  |       |          |          |                  |                  |                  |                  |                  |               |        |           |
| FirstOrDefault | Teste 1       |        18.620 ns |    NA | 0.000 ns | 0.000 ns |        18.620 ns |        18.620 ns |        18.620 ns |        18.620 ns |        18.620 ns |  53,705,600.3 | 0.0124 |     104 B |
| FirstOrDefault | Teste 568452  | 3,458,909.180 ns |    NA | 0.000 ns | 0.000 ns | 3,458,909.180 ns | 3,458,909.180 ns | 3,458,909.180 ns | 3,458,909.180 ns | 3,458,909.180 ns |         289.1 |      - |     106 B |
| FirstOrDefault | Teste 1000000 | 4,329,343.750 ns |    NA | 0.000 ns | 0.000 ns | 4,329,343.750 ns | 4,329,343.750 ns | 4,329,343.750 ns | 4,329,343.750 ns | 4,329,343.750 ns |         231.0 |      - |     116 B |

### SortedList\<string, integer\>

| Method      | FindValue     | Mean     | Error | StdErr | StdDev | Min      | Q1       | Median   | Q3       | Max      | Op/s        | Allocated |
|------------ |-------------- |---------:|------:|-------:|-------:|---------:|---------:|---------:|---------:|---------:|------------:|----------:|
| TryGetValue | Teste 1       | 677.5 ns |    NA | 0.0 ns | 0.0 ns | 677.5 ns | 677.5 ns | 677.5 ns | 677.5 ns | 677.5 ns | 1,476,098.4 |         - |
| TryGetValue | Teste 1000000 | 702.3 ns |    NA | 0.0 ns | 0.0 ns | 702.3 ns | 702.3 ns | 702.3 ns | 702.3 ns | 702.3 ns | 1,423,985.6 |         - |
| TryGetValue | Teste 568452  | 817.1 ns |    NA | 0.0 ns | 0.0 ns | 817.1 ns | 817.1 ns | 817.1 ns | 817.1 ns | 817.1 ns | 1,223,831.9 |         - |
|             |               |          |       |        |        |          |          |          |          |          |             |           |
| Contains    | Teste 1       | 664.4 ns |    NA | 0.0 ns | 0.0 ns | 664.4 ns | 664.4 ns | 664.4 ns | 664.4 ns | 664.4 ns | 1,505,047.0 |         - |
| Contains    | Teste 1000000 | 679.5 ns |    NA | 0.0 ns | 0.0 ns | 679.5 ns | 679.5 ns | 679.5 ns | 679.5 ns | 679.5 ns | 1,471,702.7 |         - |
| Contains    | Teste 568452  | 806.3 ns |    NA | 0.0 ns | 0.0 ns | 806.3 ns | 806.3 ns | 806.3 ns | 806.3 ns | 806.3 ns | 1,240,248.5 |         - |
|             |               |          |       |        |        |          |          |          |          |          |             |           |
| IndexValue  | Teste 1       | 665.0 ns |    NA | 0.0 ns | 0.0 ns | 665.0 ns | 665.0 ns | 665.0 ns | 665.0 ns | 665.0 ns | 1,503,745.8 |         - |
| IndexValue  | Teste 1000000 | 692.7 ns |    NA | 0.0 ns | 0.0 ns | 692.7 ns | 692.7 ns | 692.7 ns | 692.7 ns | 692.7 ns | 1,443,597.0 |         - |
| IndexValue  | Teste 568452  | 797.3 ns |    NA | 0.0 ns | 0.0 ns | 797.3 ns | 797.3 ns | 797.3 ns | 797.3 ns | 797.3 ns | 1,254,281.1 |         - |

## Integer

### HashSet\<integer\>

| Method         | FindValue | Mean             | Error | StdErr   | StdDev   | Min              | Q1               | Median           | Q3               | Max              | Op/s          | Gen0   | Allocated |
|--------------- |---------- |-----------------:|------:|---------:|---------:|-----------------:|-----------------:|-----------------:|-----------------:|-----------------:|--------------:|-------:|----------:|
| TryGetValue    | 1000000   |         2.391 ns |    NA | 0.000 ns | 0.000 ns |         2.391 ns |         2.391 ns |         2.391 ns |         2.391 ns |         2.391 ns | 418,178,543.3 |      - |         - |
| TryGetValue    | 568452    |         3.083 ns |    NA | 0.000 ns | 0.000 ns |         3.083 ns |         3.083 ns |         3.083 ns |         3.083 ns |         3.083 ns | 324,354,887.3 |      - |         - |
| TryGetValue    | 1         |         3.151 ns |    NA | 0.000 ns | 0.000 ns |         3.151 ns |         3.151 ns |         3.151 ns |         3.151 ns |         3.151 ns | 317,395,112.1 |      - |         - |
|                |           |                  |       |          |          |                  |                  |                  |                  |                  |               |        |           |
| Contains       | 1000000   |         2.290 ns |    NA | 0.000 ns | 0.000 ns |         2.290 ns |         2.290 ns |         2.290 ns |         2.290 ns |         2.290 ns | 436,764,064.3 |      - |         - |
| Contains       | 568452    |         2.759 ns |    NA | 0.000 ns | 0.000 ns |         2.759 ns |         2.759 ns |         2.759 ns |         2.759 ns |         2.759 ns | 362,393,413.8 |      - |         - |
| Contains       | 1         |         2.788 ns |    NA | 0.000 ns | 0.000 ns |         2.788 ns |         2.788 ns |         2.788 ns |         2.788 ns |         2.788 ns | 358,719,481.8 |      - |         - |
|                |           |                  |       |          |          |                  |                  |                  |                  |                  |               |        |           |
| FirstOrDefault | 1         |        16.257 ns |    NA | 0.000 ns | 0.000 ns |        16.257 ns |        16.257 ns |        16.257 ns |        16.257 ns |        16.257 ns |  61,510,870.4 | 0.0124 |     104 B |
| FirstOrDefault | 568452    | 1,148,096.289 ns |    NA | 0.000 ns | 0.000 ns | 1,148,096.289 ns | 1,148,096.289 ns | 1,148,096.289 ns | 1,148,096.289 ns | 1,148,096.289 ns |         871.0 |      - |     105 B |
| FirstOrDefault | 1000000   | 2,070,241.797 ns |    NA | 0.000 ns | 0.000 ns | 2,070,241.797 ns | 2,070,241.797 ns | 2,070,241.797 ns | 2,070,241.797 ns | 2,070,241.797 ns |         483.0 |      - |     106 B |

## Legends

FindValue : Value of the 'FindValue' parameter
Mean      : Arithmetic mean of all measurements
Error     : Half of 99.9% confidence interval
StdErr    : Standard error of all measurements
StdDev    : Standard deviation of all measurements
Min       : Minimum
Q1        : Quartile 1 (25th percentile)
Median    : Value separating the higher half of all measurements (50th percentile)
Q3        : Quartile 3 (75th percentile)
Max       : Maximum
Op/s      : Operation per second
Allocated : Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)
1 ns      : 1 Nanosecond (0.000000001 sec)