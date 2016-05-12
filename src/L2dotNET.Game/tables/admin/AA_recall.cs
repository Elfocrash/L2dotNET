namespace L2dotNET.GameService.tables.admin
{
    class AA_recall : _adminAlias
    {
        public AA_recall()
        {
            cmd = "recall";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //recall [ник] -- призывает к себе чара с ником [ник]
        }
    }
}
