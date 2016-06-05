namespace L2dotNET.GameService.tables.admin
{
    class AA_delskill : _adminAlias
    {
        public AA_delskill()
        {
            cmd = "delskill";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //delskill [skill id] -- забирает скилл [skill_id] у выбранного чара
        }
    }
}