namespace L2dotNET.GameService.tables.admin
{
    class AA_setclass : _adminAlias
    {
        public AA_setclass()
        {
            cmd = "setclass";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //setclass [class_id] -- устанавливает профессию [class_id] выбранному чару
        }
    }
}