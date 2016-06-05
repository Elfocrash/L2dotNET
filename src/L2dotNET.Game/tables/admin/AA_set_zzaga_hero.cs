namespace L2dotNET.GameService.tables.admin
{
    class AA_set_zzaga_hero : _adminAlias
    {
        public AA_set_zzaga_hero()
        {
            cmd = "set_zzaga_hero";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //set_zzaga_hero -- временно дает статус героя
        }
    }
}