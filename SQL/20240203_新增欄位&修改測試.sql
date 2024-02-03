--�ϥθ�Ʈw
Use Worksheet2023

--�����j�M
select *
from dbo.Raw$

--�s�Wunit��Estimation���
alter table dbo.Raw$ add unit int, Estimation int

--�ק�unit��Estimation���e(�O�n�ھڤ���i��ק�?)
update dbo.Raw$
set unit=0
where Brand='' AND Family='' AND Model='' AND Display_size='' AND Display_resolution=''

update dbo.Raw$
set Estimation=0
where Brand='' AND Family='' AND Model='' AND Display_size='' AND Display_resolution=''

--feature�հ�(family)
select Brand, category, Family, count(SKU) AS SKU_Count, CONCAT(CAST(ROUND(COUNT(SKU) * 100.0 / SUM(COUNT(SKU)) OVER(PARTITION BY Brand), 2) AS DECIMAL(18, 2)),'%') AS SKU_Pct
from dbo.Raw$
--where Brand='Acer' OR Brand='HP'
group by Brand, category, Family
order by Brand, category, Family

--feature�հ�(size)
select Brand, Size,  count(SKU) AS SKU_Count, CONCAT(CAST(ROUND(COUNT(SKU) * 100.0 / SUM(COUNT(SKU)) OVER(PARTITION BY Brand), 2) AS DECIMAL(18, 2)),'%') AS SKU_Pct
from dbo.Raw$
where Brand='Acer' OR Brand='HP'
group by Brand, Size
order by Brand, Size

--feature�հ�(CPU)
select Brand, [CPU-Gen], count(SKU) AS SKU_Count, CONCAT(CAST(ROUND(COUNT(SKU) * 100.0 / SUM(COUNT(SKU)) OVER(PARTITION BY Brand), 2) AS DECIMAL(18, 2)),'%') AS SKU_Pct
from dbo.Raw$
where Brand='Acer' OR Brand='HP'
group by Brand, [CPU-Gen]
order by Brand, [CPU-Gen]

--feature�հ�(GPU)
select Brand, GPU_Group, count(SKU) AS SKU_Count, CONCAT(CAST(ROUND(COUNT(SKU) * 100.0 / SUM(COUNT(SKU)) OVER(PARTITION BY Brand), 2) AS DECIMAL(18, 2)),'%') AS SKU_Pct
from dbo.Raw$
where Brand='Acer' OR Brand='HP'
group by Brand, GPU_Group
order by Brand, GPU_Group

--feature�հ�(display)
select Brand, Size, Display_resolution, count(SKU) AS SKU_Count, CONCAT(CAST(ROUND(COUNT(SKU) * 100.0 / SUM(COUNT(SKU)) OVER(PARTITION BY Brand), 2) AS DECIMAL(18, 2)),'%') AS SKU_Pct
from dbo.Raw$
where Brand='Acer' OR Brand='HP'
group by Brand, Size, Display_resolution
order by Brand, Size, Display_resolution