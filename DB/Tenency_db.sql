create table Property (
guid uuid primary key default gen_random_uuid(),
property_name varchar(150) not null,
address varchar(500) not null,
length_value decimal(10, 4),
width_value decimal(10, 4),
unit VARCHAR(10) CHECK (unit IN ('feet', 'meters', 'inches', 'cm')),
base_rent decimal(12,2) not null,
base_current_price decimal(10,2) not null,
created_at timestamp not null,
updated_at timestamp
);
Create Table Tenant(
guid uuid primary key default gen_random_uuid(),
property_id uuid References property(guid) not null,
tenant_name varchar(200) not null,
proof_number varchar(100) not null,
rent_type varchar(50) check(rent_type in ('commercial', 'house')) not null,
shop_name varchar(300),
rent_start_date date not null,
move_in_date date not null,
end_date date,
starting_month_current numeric(12,2) not null,
created_at timestamp not null,
is_active boolean not null,
updated_at timestamp
);
create table Monthly_Details(
guid uuid primary key default gen_random_uuid(),
tenant_id uuid references tenant(guid) not null,
property_id uuid references property(guid) not null,
billing_month DATE not null,
current_used numeric(12,2) not null,
Current_reading_from numeric(12,2) not null,
Current_reading_to numeric(12,2) not null,
total_montly_rent numeric(12,2) not null,
electricity_charges numeric(12,2) not null,
amount_paid numeric(12,2) not null,
due numeric(12,2) not null,
created_at timestamp not null,
updated_at timestamp
);
create table expense(
guid uuid primary key default gen_random_uuid(),
property_id uuid references property(guid) not null,
amount_spent numeric(10,2) not null,
expense_date DATE NOT NULL,
description varchar(500) not null,
created_at timestamp not null,
updated_at timestamp
)