-- drop table if exists teste;
-- create table teste as table transacao; -- copia com dados e sem indices

create table teste (like transacao including all);
insert into teste select * from transacao;

select * from teste where id = '0199310e-714e-7cfe-9d15-44eeba338f59';

insert into public.teste (id,"data",cliente_id,valor,descricao) 
values ('0199310e-714e-7cfe-9d15-44eeba338f59'::uuid,'2025-08-23 00:39:19.264','0199310e-714e-782b-b756-83bc6bcedfec'::uuid,1743.54,'Ea maxime dolores non sint omnis volupta')
on conflict (id) do update
--on conflict on constraint teste_pkey do update
set
	"data" = '2025-08-23 00:39:19.264',
	cliente_id = '0199310e-714e-782b-b756-83bc6bcedfec',
	valor = 1800.00,
	descricao = 'teste 3';

insert into public.teste (id,"data",cliente_id,valor,descricao) 
values ('0199310e-714e-7cfe-9d15-44eeba338f59'::uuid,'2025-08-23 00:39:19.264','0199310e-714e-782b-b756-83bc6bcedfec'::uuid,1743.54,'novo valor')
on conflict (id) do update
--on conflict on constraint teste_pkey do update
set
	"data" = excluded."data",
	cliente_id = excluded.cliente_id,
	valor = excluded.valor,
	descricao = excluded.descricao;


-- bulk upsert
create temp table transacao_temp on commit drop as table transacao with no data;


truncate transacao;

insert into transacao
select * from teste
on conflict (id) do update
set
	"data" = excluded."data",
	cliente_id = excluded.cliente_id,
	valor = excluded.valor,
	descricao = '(atualizado)' || substring(excluded.descricao, 1, 25);

select count(*) from transacao;
select * from transacao where descricao ilike '%atualizado%';

select * from transacao_temp;
