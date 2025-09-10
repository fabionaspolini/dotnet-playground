drop table if exists public.transacao_part;

create table public.transacao_part (
	id uuid not null,
	"data" timestamptz not null,
	cliente_id uuid not null,
	valor numeric(12, 2) not null,
	descricao varchar(40) not null,
	data_atualizacao timestamptz null,
	constraint pk_transacao_part primary key (id, "data")
) partition by range ("data");
create index ix_transacao_part_cliente_id on public.transacao_part using btree (cliente_id);
create index ix_transacao_part_data on public.transacao_part using btree (data);
create index ix_transacao_part_data_atualizacao on public.transacao_part using btree (data_atualizacao);

create table transacao_part_2024_09 partition of transacao_part for values from ('2024-09-01') to ('2024-10-01');
create table transacao_part_2024_10 partition of transacao_part for values from ('2024-10-01') to ('2024-11-01');
create table transacao_part_2024_11 partition of transacao_part for values from ('2024-11-01') to ('2024-12-01');
create table transacao_part_2024_12 partition of transacao_part for values from ('2024-12-01') to ('2025-01-01');

create table transacao_part_2025_01 partition of transacao_part for values from ('2025-01-01') to ('2025-02-01');
create table transacao_part_2025_02 partition of transacao_part for values from ('2025-02-01') to ('2025-03-01');
create table transacao_part_2025_03 partition of transacao_part for values from ('2025-03-01') to ('2025-04-01');
create table transacao_part_2025_04 partition of transacao_part for values from ('2025-04-01') to ('2025-05-01');
create table transacao_part_2025_05 partition of transacao_part for values from ('2025-05-01') to ('2025-06-01');
create table transacao_part_2025_06 partition of transacao_part for values from ('2025-06-01') to ('2025-07-01');
create table transacao_part_2025_07 partition of transacao_part for values from ('2025-07-01') to ('2025-08-01');
create table transacao_part_2025_08 partition of transacao_part for values from ('2025-08-01') to ('2025-09-01');
create table transacao_part_2025_09 partition of transacao_part for values from ('2025-09-01') to ('2025-10-01');
create table transacao_part_2025_10 partition of transacao_part for values from ('2025-10-01') to ('2025-11-01');
create table transacao_part_2025_11 partition of transacao_part for values from ('2025-11-01') to ('2025-12-01');
create table transacao_part_2025_12 partition of transacao_part for values from ('2025-12-01') to ('2026-01-01');

-- inserir
truncate transacao_part;

insert into transacao_part
select * from transacao;

vacuum;

select count(*) from transacao;
select count(*) from transacao_part;



select * from transacao where "data" between '2025-08-01' and '2025-09-10' and valor >= 1000 order by "data" limit 1000;
select * from transacao_part where "data" between '2025-08-01' and '2025-09-10' and valor >= 1000 order by "data" limit 1000;

select count(*), sum(valor) from transacao where "data" between '2025-08-01' and '2025-09-10' and valor >= 1000;
select count(*), sum(valor) from transacao_part where "data" between '2025-08-01' and '2025-09-10' and valor >= 1000;


