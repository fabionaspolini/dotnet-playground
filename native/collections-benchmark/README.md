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

Pesquisa em coleção com 1 milhão de strings.

### ConcurrentDictionary\<string, integer\>

| Method            | FindValue     | Mean      | Error | StdErr   | StdDev   | Min       | Q1        | Median    | Q3        | Max       | Op/s          | Allocated |
|------------------ |-------------- |----------:|------:|---------:|---------:|----------:|----------:|----------:|----------:|----------:|--------------:|----------:|
| TryGetValue       | Teste 1       |  8.880 ns |    NA | 0.000 ns | 0.000 ns |  8.880 ns |  8.880 ns |  8.880 ns |  8.880 ns |  8.880 ns | 112,607,947.6 |         - |
| TryGetValue       | Teste 568452  |  9.379 ns |    NA | 0.000 ns | 0.000 ns |  9.379 ns |  9.379 ns |  9.379 ns |  9.379 ns |  9.379 ns | 106,616,488.9 |         - |
| TryGetValue       | Teste 1000000 |  9.675 ns |    NA | 0.000 ns | 0.000 ns |  9.675 ns |  9.675 ns |  9.675 ns |  9.675 ns |  9.675 ns | 103,355,346.2 |         - |
|                   |               |           |       |          |          |           |           |           |           |           |               |           |
| GetValueOrDefault | Teste 1       |  9.237 ns |    NA | 0.000 ns | 0.000 ns |  9.237 ns |  9.237 ns |  9.237 ns |  9.237 ns |  9.237 ns | 108,254,595.4 |         - |
| GetValueOrDefault | Teste 568452  |  9.749 ns |    NA | 0.000 ns | 0.000 ns |  9.749 ns |  9.749 ns |  9.749 ns |  9.749 ns |  9.749 ns | 102,574,500.0 |         - |
| GetValueOrDefault | Teste 1000000 | 10.035 ns |    NA | 0.000 ns | 0.000 ns | 10.035 ns | 10.035 ns | 10.035 ns | 10.035 ns | 10.035 ns |  99,651,065.4 |         - |
|                   |               |           |       |          |          |           |           |           |           |           |               |           |
| Contains          | Teste 1       |  8.874 ns |    NA | 0.000 ns | 0.000 ns |  8.874 ns |  8.874 ns |  8.874 ns |  8.874 ns |  8.874 ns | 112,693,476.8 |         - |
| Contains          | Teste 568452  |  9.430 ns |    NA | 0.000 ns | 0.000 ns |  9.430 ns |  9.430 ns |  9.430 ns |  9.430 ns |  9.430 ns | 106,039,811.3 |         - |
| Contains          | Teste 1000000 |  9.651 ns |    NA | 0.000 ns | 0.000 ns |  9.651 ns |  9.651 ns |  9.651 ns |  9.651 ns |  9.651 ns | 103,612,261.6 |         - |
|                   |               |           |       |          |          |           |           |           |           |           |               |           |
| IndexValue        | Teste 1       |  9.605 ns |    NA | 0.000 ns | 0.000 ns |  9.605 ns |  9.605 ns |  9.605 ns |  9.605 ns |  9.605 ns | 104,109,314.3 |         - |
| IndexValue        | Teste 1000000 | 10.287 ns |    NA | 0.000 ns | 0.000 ns | 10.287 ns | 10.287 ns | 10.287 ns | 10.287 ns | 10.287 ns |  97,207,351.1 |         - |
| IndexValue        | Teste 568452  | 10.372 ns |    NA | 0.000 ns | 0.000 ns | 10.372 ns | 10.372 ns | 10.372 ns | 10.372 ns | 10.372 ns |  96,408,979.8 |         - |


### Dictionary\<string, int\>

| Method            | FindValue     | Mean      | Error | StdErr   | StdDev   | Min       | Q1        | Median    | Q3        | Max       | Op/s          | Allocated |
|------------------ |-------------- |----------:|------:|---------:|---------:|----------:|----------:|----------:|----------:|----------:|--------------:|----------:|
| TryGetValue       | Teste 1       |  9.750 ns |    NA | 0.000 ns | 0.000 ns |  9.750 ns |  9.750 ns |  9.750 ns |  9.750 ns |  9.750 ns | 102,561,143.8 |         - |
| TryGetValue       | Teste 568452  | 10.017 ns |    NA | 0.000 ns | 0.000 ns | 10.017 ns | 10.017 ns | 10.017 ns | 10.017 ns | 10.017 ns |  99,832,247.4 |         - |
| TryGetValue       | Teste 1000000 | 10.140 ns |    NA | 0.000 ns | 0.000 ns | 10.140 ns | 10.140 ns | 10.140 ns | 10.140 ns | 10.140 ns |  98,622,312.2 |         - |
|                   |               |           |       |          |          |           |           |           |           |           |               |           |
| GetValueOrDefault | Teste 568452  | 11.124 ns |    NA | 0.000 ns | 0.000 ns | 11.124 ns | 11.124 ns | 11.124 ns | 11.124 ns | 11.124 ns |  89,894,457.0 |         - |
| GetValueOrDefault | Teste 1       | 11.202 ns |    NA | 0.000 ns | 0.000 ns | 11.202 ns | 11.202 ns | 11.202 ns | 11.202 ns | 11.202 ns |  89,269,725.1 |         - |
| GetValueOrDefault | Teste 1000000 | 11.452 ns |    NA | 0.000 ns | 0.000 ns | 11.452 ns | 11.452 ns | 11.452 ns | 11.452 ns | 11.452 ns |  87,318,470.9 |         - |
|                   |               |           |       |          |          |           |           |           |           |           |               |           |
| Contains          | Teste 1       |  9.884 ns |    NA | 0.000 ns | 0.000 ns |  9.884 ns |  9.884 ns |  9.884 ns |  9.884 ns |  9.884 ns | 101,174,347.9 |         - |
| Contains          | Teste 568452  |  9.941 ns |    NA | 0.000 ns | 0.000 ns |  9.941 ns |  9.941 ns |  9.941 ns |  9.941 ns |  9.941 ns | 100,597,862.1 |         - |
| Contains          | Teste 1000000 |  9.965 ns |    NA | 0.000 ns | 0.000 ns |  9.965 ns |  9.965 ns |  9.965 ns |  9.965 ns |  9.965 ns | 100,354,640.3 |         - |
|                   |               |           |       |          |          |           |           |           |           |           |               |           |
| IndexValue        | Teste 1       | 10.754 ns |    NA | 0.000 ns | 0.000 ns | 10.754 ns | 10.754 ns | 10.754 ns | 10.754 ns | 10.754 ns |  92,991,248.3 |         - |
| IndexValue        | Teste 568452  | 10.797 ns |    NA | 0.000 ns | 0.000 ns | 10.797 ns | 10.797 ns | 10.797 ns | 10.797 ns | 10.797 ns |  92,619,892.8 |         - |
| IndexValue        | Teste 1000000 | 10.820 ns |    NA | 0.000 ns | 0.000 ns | 10.820 ns | 10.820 ns | 10.820 ns | 10.820 ns | 10.820 ns |  92,425,312.4 |         - |

### HashSet\<string\>

| Method         | FindValue     | Mean             | Error | StdErr   | StdDev   | Min              | Q1               | Median           | Q3               | Max              | Op/s          | Gen0   | Allocated |
|--------------- |-------------- |-----------------:|------:|---------:|---------:|-----------------:|-----------------:|-----------------:|-----------------:|-----------------:|--------------:|-------:|----------:|
| TryGetValue    | Teste 1       |        10.459 ns |    NA | 0.000 ns | 0.000 ns |        10.459 ns |        10.459 ns |        10.459 ns |        10.459 ns |        10.459 ns |  95,609,161.5 |      - |         - |
| TryGetValue    | Teste 568452  |        10.503 ns |    NA | 0.000 ns | 0.000 ns |        10.503 ns |        10.503 ns |        10.503 ns |        10.503 ns |        10.503 ns |  95,208,433.5 |      - |         - |
| TryGetValue    | Teste 1000000 |        10.577 ns |    NA | 0.000 ns | 0.000 ns |        10.577 ns |        10.577 ns |        10.577 ns |        10.577 ns |        10.577 ns |  94,542,123.7 |      - |         - |
|                |               |                  |       |          |          |                  |                  |                  |                  |                  |               |        |           |
| Contains       | Teste 568452  |         9.462 ns |    NA | 0.000 ns | 0.000 ns |         9.462 ns |         9.462 ns |         9.462 ns |         9.462 ns |         9.462 ns | 105,687,061.8 |      - |         - |
| Contains       | Teste 1       |         9.831 ns |    NA | 0.000 ns | 0.000 ns |         9.831 ns |         9.831 ns |         9.831 ns |         9.831 ns |         9.831 ns | 101,720,184.0 |      - |         - |
| Contains       | Teste 1000000 |        10.480 ns |    NA | 0.000 ns | 0.000 ns |        10.480 ns |        10.480 ns |        10.480 ns |        10.480 ns |        10.480 ns |  95,422,261.7 |      - |         - |
|                |               |                  |       |          |          |                  |                  |                  |                  |                  |               |        |           |
| FirstOrDefault | Teste 1       |        17.613 ns |    NA | 0.000 ns | 0.000 ns |        17.613 ns |        17.613 ns |        17.613 ns |        17.613 ns |        17.613 ns |  56,777,072.1 | 0.0124 |     104 B |
| FirstOrDefault | Teste 568452  | 3,793,210.938 ns |    NA | 0.000 ns | 0.000 ns | 3,793,210.938 ns | 3,793,210.938 ns | 3,793,210.938 ns | 3,793,210.938 ns | 3,793,210.938 ns |         263.6 |      - |     110 B |
| FirstOrDefault | Teste 1000000 | 4,594,320.312 ns |    NA | 0.000 ns | 0.000 ns | 4,594,320.312 ns | 4,594,320.312 ns | 4,594,320.312 ns | 4,594,320.312 ns | 4,594,320.312 ns |         217.7 |      - |     116 B |

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

| Method            | FindValue     | Mean     | Error | StdErr | StdDev | Min      | Q1       | Median   | Q3       | Max      | Op/s        | Allocated |
|------------------ |-------------- |---------:|------:|-------:|-------:|---------:|---------:|---------:|---------:|---------:|------------:|----------:|
| TryGetValue       | Teste 1000000 | 656.7 ns |    NA | 0.0 ns | 0.0 ns | 656.7 ns | 656.7 ns | 656.7 ns | 656.7 ns | 656.7 ns | 1,522,779.7 |         - |
| TryGetValue       | Teste 1       | 670.2 ns |    NA | 0.0 ns | 0.0 ns | 670.2 ns | 670.2 ns | 670.2 ns | 670.2 ns | 670.2 ns | 1,492,059.0 |         - |
| TryGetValue       | Teste 568452  | 952.1 ns |    NA | 0.0 ns | 0.0 ns | 952.1 ns | 952.1 ns | 952.1 ns | 952.1 ns | 952.1 ns | 1,050,342.9 |         - |
|                   |               |          |       |        |        |          |          |          |          |          |             |           |
| GetValueOrDefault | Teste 1       | 661.9 ns |    NA | 0.0 ns | 0.0 ns | 661.9 ns | 661.9 ns | 661.9 ns | 661.9 ns | 661.9 ns | 1,510,899.0 |         - |
| GetValueOrDefault | Teste 1000000 | 782.4 ns |    NA | 0.0 ns | 0.0 ns | 782.4 ns | 782.4 ns | 782.4 ns | 782.4 ns | 782.4 ns | 1,278,131.4 |         - |
| GetValueOrDefault | Teste 568452  | 887.6 ns |    NA | 0.0 ns | 0.0 ns | 887.6 ns | 887.6 ns | 887.6 ns | 887.6 ns | 887.6 ns | 1,126,636.3 |         - |
|                   |               |          |       |        |        |          |          |          |          |          |             |           |
| Contains          | Teste 1       | 664.2 ns |    NA | 0.0 ns | 0.0 ns | 664.2 ns | 664.2 ns | 664.2 ns | 664.2 ns | 664.2 ns | 1,505,542.4 |         - |
| Contains          | Teste 1000000 | 689.2 ns |    NA | 0.0 ns | 0.0 ns | 689.2 ns | 689.2 ns | 689.2 ns | 689.2 ns | 689.2 ns | 1,450,945.7 |         - |
| Contains          | Teste 568452  | 840.9 ns |    NA | 0.0 ns | 0.0 ns | 840.9 ns | 840.9 ns | 840.9 ns | 840.9 ns | 840.9 ns | 1,189,178.1 |         - |
|                   |               |          |       |        |        |          |          |          |          |          |             |           |
| IndexValue        | Teste 1       | 652.8 ns |    NA | 0.0 ns | 0.0 ns | 652.8 ns | 652.8 ns | 652.8 ns | 652.8 ns | 652.8 ns | 1,531,810.9 |         - |
| IndexValue        | Teste 1000000 | 704.6 ns |    NA | 0.0 ns | 0.0 ns | 704.6 ns | 704.6 ns | 704.6 ns | 704.6 ns | 704.6 ns | 1,419,232.6 |         - |
| IndexValue        | Teste 568452  | 818.0 ns |    NA | 0.0 ns | 0.0 ns | 818.0 ns | 818.0 ns | 818.0 ns | 818.0 ns | 818.0 ns | 1,222,495.1 |         - |

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

```
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
```

**Summary**

```
BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.4317/23H2/2023Update/SunValley3)
AMD Ryzen 5 5600G with Radeon Graphics, 1 CPU, 12 logical and 6 physical cores
.NET SDK 8.0.403
  [Host]            : .NET 8.0.10 (8.0.1024.46610), X64 RyuJIT AVX2 [AttachedDebugger]
  ShortRun-.NET 8.0 : .NET 8.0.10 (8.0.1024.46610), X64 RyuJIT AVX2

Job=ShortRun-.NET 8.0  Runtime=.NET 8.0  IterationCount=1  
LaunchCount=1  WarmupCount=3  
```