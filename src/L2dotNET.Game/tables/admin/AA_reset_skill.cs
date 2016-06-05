namespace L2dotNET.GameService.tables.admin
{
    class AA_reset_skill : _adminAlias
    {
        public AA_reset_skill()
        {
            cmd = "reset_skill";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //reset_skill - удалить все скилы цели
        }
    }
}