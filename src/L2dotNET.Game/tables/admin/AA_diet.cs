namespace L2dotNET.GameService.tables.admin
{
    class AA_diet : _adminAlias
    {
        public AA_diet()
        {
            cmd = "diet";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //diet [on|off] -- включение\отключения лимита веса
            bool changed = false;
            switch (alias.Split(' ')[1])
            {
                case "on":
                    changed = admin._diet = false;
                    admin._diet = true;
                    admin.sendMessage("Diet mode on.");
                    break;
                case "off":
                    changed = admin._diet = true;
                    admin._diet = false;
                    admin.sendMessage("Diet mode off.");
                    break;
            }

            if (changed)
                admin.updateWeight();
        }
    }
}