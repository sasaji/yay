CREATE TABLE Edit_ObjectInfo(
    object_id                  uniqueidentifier    NOT NULL,
    object_type_id             nvarchar(30)        NOT NULL,
    basicinfo_id               uniqueidentifier    NOT NULL,
    domain_id                  int                 NOT NULL,
    ou_id                      int                 NULL,
    cn                         nvarchar(256)       NOT NULL,
    display_name               nvarchar(64)        NULL,
    logon_name                 nvarchar(256)       NULL,
    status                     nchar(1)            NULL,
    result                     int                 NULL,
    info                       nvarchar(1024)      NULL,
    mail_nickname              nvarchar(64)        NULL,
    address_lists_flag         bit                 NULL,
    policies_excluded_flag     bit                 NULL,
    alt_recipient_object_id    uniqueidentifier    NULL,
    manager_object_id          uniqueidentifier    NULL,
    account_expires            datetime            NULL,
    control_flag               bit                 NULL,
    regist_date                datetime            NOT NULL,
    update_date                datetime            NULL,
    delete_date                datetime            NULL,
    objectinfo_data            xml                NOT NULL,
    object_guid                nvarchar(100)       NULL,
    CONSTRAINT PK39_1 PRIMARY KEY CLUSTERED (object_id)
)


