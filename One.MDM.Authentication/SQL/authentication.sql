DROP TABLE IF EXISTS public.user CASCADE;
DROP TABLE IF EXISTS public.role CASCADE;
DROP TABLE IF EXISTS public.user_role CASCADE;
DROP TABLE IF EXISTS public.privilege CASCADE;
DROP TABLE public.entities CASCADE;
CREATE EXTENSION IF NOT EXISTS "pgcrypto";
CREATE TABLE public.user (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_name VARCHAR(250) NOT NULL,
    password VARCHAR(250) NOT NULL,
    is_active BIGINT DEFAULT 1,
    created_date TIMESTAMP WITHOUT TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    created_by VARCHAR(250),
    modified_date TIMESTAMP WITHOUT TIME ZONE,
    modified_by VARCHAR(250)
);

CREATE TABLE public.role (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(250) NOT NULL,
    created_date TIMESTAMP WITHOUT TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    created_by VARCHAR(250),
    modified_date TIMESTAMP WITHOUT TIME ZONE,
    modified_by VARCHAR(250)
);

CREATE TABLE public.user_role (
    user_id UUID NOT NULL,
    role_id UUID NOT NULL,
    CONSTRAINT fk_user_role_user FOREIGN KEY (user_id) REFERENCES public.user(id) ON DELETE CASCADE,
    CONSTRAINT fk_user_role_role FOREIGN KEY (role_id) REFERENCES public.role(id) ON DELETE CASCADE
);

CREATE TABLE public.entities (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    name VARCHAR(250) NOT NULL,
    created_date TIMESTAMP WITHOUT TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    created_by VARCHAR(250),
    modified_date TIMESTAMP WITHOUT TIME ZONE,
    modified_by VARCHAR(250)
);
CREATE TABLE public.privilege (
    id UUID DEFAULT gen_random_uuid() PRIMARY KEY,
    role_id UUID NOT NULL,
    table_id BIGINT NOT NULL, 
    table_name VARCHAR(250) NOT NULL,
    view BIGINT DEFAULT 0,
    read BIGINT DEFAULT 0,
    write BIGINT DEFAULT 0,
    update BIGINT DEFAULT 0,
    delete BIGINT DEFAULT 0,
    approve BIGINT DEFAULT 0,
    assign BIGINT DEFAULT 0,
    share BIGINT DEFAULT 0,

    created_date TIMESTAMP WITHOUT TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    created_by VARCHAR(250),
    modified_date TIMESTAMP WITHOUT TIME ZONE,
    modified_by VARCHAR(250),

    CONSTRAINT fk_privilege_role FOREIGN KEY (role_id) REFERENCES public.role(id) ON DELETE CASCADE,
    CONSTRAINT fk_privilege_entities FOREIGN KEY (table_id) REFERENCES public.entities(id) ON DELETE CASCADE
);




-- Privilege cho bảng user
INSERT INTO public.user
    (id, user_name, password, created_date, modified_date, created_by, modified_by)
VALUES 
    (gen_random_uuid(), 'admin', '$2a$11$3RzlSXULb7/pRHUFwZCyJO7ySM9ysP1eAX5tlGrantocdFkKmOpWa', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'system', 'system');

-- Privilege cho bảng role

INSERT INTO public.role
    (id, name, created_date, modified_date, created_by, modified_by)
VALUES
    (
        gen_random_uuid(),
        'System Admin',
        CURRENT_TIMESTAMP,
        CURRENT_TIMESTAMP,
        'system',
        'system'
    );

INSERT INTO public.user_role
    (user_id, role_id)
VALUES 
    ((SELECT id FROM public."user" WHERE user_name = 'admin'), (SELECT id FROM public.role WHERE name = 'System Admin'));

--- entities
INSERT INTO public.entities (table_name, created_by, modified_date, modified_by)
VALUES
    ('branch', 'System', CURRENT_TIMESTAMP, 'Admin'),
    ('formula', 'System', CURRENT_TIMESTAMP, 'Admin'),
    ('formula_component', 'System', CURRENT_TIMESTAMP, 'Admin'),
    ('inventory', 'System', CURRENT_TIMESTAMP, 'Admin'),
    ('product', 'System', CURRENT_TIMESTAMP, 'Admin'),
    ('product_prices', 'System', CURRENT_TIMESTAMP, 'Admin'),
    ('stock_transactions', 'System', CURRENT_TIMESTAMP, 'Admin'),
    ('unit', 'System', CURRENT_TIMESTAMP, 'Admin'),
    ('unit_conversion', 'System', CURRENT_TIMESTAMP, 'Admin'),
    ('warehouse', 'System', CURRENT_TIMESTAMP, 'Admin');
    ('entities', 'System', CURRENT_TIMESTAMP, 'Admin');

-- Privilege cho bảng privilege
-- branch
INSERT INTO public.privilege
(id, role_id, table_name, read, write, update, delete, approve, assign, share,
 created_date, modified_date, created_by, modified_by)
VALUES (gen_random_uuid(), (SELECT id FROM public."role" WHERE name = 'System Admin'),
 'branch', 1,1,1,1,1,1,1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'system', 'system');

-- formula
INSERT INTO public.privilege
(id, role_id, table_name, read, write, update, delete, approve, assign, share,
 created_date, modified_date, created_by, modified_by)
VALUES (gen_random_uuid(), (SELECT id FROM public."role" WHERE name = 'System Admin'),
 'formula', 1,1,1,1,1,1,1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'system', 'system');

-- formula_component
INSERT INTO public.privilege
(id, role_id, table_name, read, write, update, delete, approve, assign, share,
 created_date, modified_date, created_by, modified_by)
VALUES (gen_random_uuid(), (SELECT id FROM public."role" WHERE name = 'System Admin'),
 'formula_component', 1,1,1,1,1,1,1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'system', 'system');

-- inventory
INSERT INTO public.privilege
(id, role_id, table_name, read, write, update, delete, approve, assign, share,
 created_date, modified_date, created_by, modified_by)
VALUES (gen_random_uuid(), (SELECT id FROM public."role" WHERE name = 'System Admin'),
 'inventory', 1,1,1,1,1,1,1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'system', 'system');

-- product
INSERT INTO public.privilege
(id, role_id, table_name, read, write, update, delete, approve, assign, share,
 created_date, modified_date, created_by, modified_by)
VALUES (gen_random_uuid(), (SELECT id FROM public."role" WHERE name = 'System Admin'),
 'product', 1,1,1,1,1,1,1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'system', 'system');

-- product_prices
INSERT INTO public.privilege
(id, role_id, table_name, read, write, update, delete, approve, assign, share,
 created_date, modified_date, created_by, modified_by)
VALUES (gen_random_uuid(), (SELECT id FROM public."role" WHERE name = 'System Admin'),
 'product_prices', 1,1,1,1,1,1,1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'system', 'system');

-- stock_transactions
INSERT INTO public.privilege
(id, role_id, table_name, read, write, update, delete, approve, assign, share,
 created_date, modified_date, created_by, modified_by)
VALUES (gen_random_uuid(), (SELECT id FROM public."role" WHERE name = 'System Admin'),
 'stock_transactions', 1,1,1,1,1,1,1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'system', 'system');

-- unit
INSERT INTO public.privilege
(id, role_id, table_name, read, write, update, delete, approve, assign, share,
 created_date, modified_date, created_by, modified_by)
VALUES (gen_random_uuid(), (SELECT id FROM public."role" WHERE name = 'System Admin'),
 'unit', 1,1,1,1,1,1,1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'system', 'system');

-- unit_conversion
INSERT INTO public.privilege
(id, role_id, table_name, read, write, update, delete, approve, assign, share,
 created_date, modified_date, created_by, modified_by)
VALUES (gen_random_uuid(), (SELECT id FROM public."role" WHERE name = 'System Admin'),
 'unit_conversion', 1,1,1,1,1,1,1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'system', 'system');

-- warehouse
INSERT INTO public.privilege
(id, role_id, table_name, read, write, update, delete, approve, assign, share,
 created_date, modified_date, created_by, modified_by)
VALUES (gen_random_uuid(), (SELECT id FROM public."role" WHERE name = 'System Admin'),
 'warehouse', 1,1,1,1,1,1,1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'system', 'system');
 -- entities
INSERT INTO public.privilege
(id, role_id, table_name, read, write, update, delete, approve, assign, share,
 created_date, modified_date, created_by, modified_by)
VALUES (gen_random_uuid(), (SELECT id FROM public."role" WHERE name = 'System Admin'),
 'entities', 1,1,1,1,1,1,1, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, 'system', 'system');
