namespace L2dotNET.GameService.tables.admin
{
    class AA_gmon : _adminAlias
    {
        public AA_gmon()
        {
            cmd = "gmon";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //gmon -- это команда начала ) содержит в себе несколько команд - диета, отключение приватов, внесение в сиписок Гмов, скорость, хайд и запрещение принятие в партию, бессмертие.
        }
    }
}
