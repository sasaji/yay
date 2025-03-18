CREATE TABLE Edit_ObjectInfo_MailAddress(
    object_id       uniqueidentifier    NOT NULL,
    protocol        nvarchar(10)        NOT NULL,
    sort_no         int                 NOT NULL,
    mail_address    nvarchar(1024)      NOT NULL,
    CONSTRAINT PK91_3_1 PRIMARY KEY CLUSTERED (object_id, protocol, sort_no)
)

