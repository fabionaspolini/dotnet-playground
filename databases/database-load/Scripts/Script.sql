select count(*) from transacao t;

select * from transacao order by 1 desc;
select * from transacao where id = '0199279b-7706-7fd4-8ac0-60ad5810ee1d';
select 1 from transacao t where 1 = 2;

select count(*) from transacao t where t.descricao ilike '%atuali%';
select * from transacao t where t.descricao ilike '%atuali%';
select count(*) from transacao t where t.data_atualizacao is not null;

delete from transacao t where t.data_atualizacao is null;

select min(t.data), max(t."data" ) from transacao t; 

insert into transacao (id, cliente_id, valor, descricao)
values
('0199274d-5e3a-7cb9-a32d-1ab0c007e82d', '0199274d-5e3c-7065-9d22-a13838d78e5f', 709.89, 'Quibusdam et molestiae qui soluta repreh'),
('0199274d-5e41-7362-a1f9-6c75335a8c0a', '0199274d-5e41-7c5d-b693-89b2697b3531', 4425.45, 'Quam nobis aut eum at voluptatem eveniet');


select now()

select current_timestamp