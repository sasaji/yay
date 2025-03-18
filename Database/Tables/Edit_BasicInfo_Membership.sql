CREATE TABLE Edit_BasicInfo_Membership(
    basicinfo_id           uniqueidentifier    NOT NULL,
    parent_basicinfo_id    uniqueidentifier    NOT NULL,
    sort_no                int                 NOT NULL,
    add_date               datetime            NOT NULL,
    delete_mode            bit                 NULL,
    CONSTRAINT PK100_1 PRIMARY KEY CLUSTERED (basicinfo_id, parent_basicinfo_id, sort_no)
)

