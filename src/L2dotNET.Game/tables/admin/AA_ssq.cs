namespace L2dotNET.GameService.tables.admin
{
    class AA_ssq : _adminAlias
    {
        public AA_ssq()
        {
            cmd = "ssq";
        }

        protected internal override void use(L2Player admin, string alias)
        {
            //ssq cyclemode default 2 -- запуск 7 печатей в стандартном режиме
            //ssq cyclemode manual 2 -- запуск 7 печатей в ручном режиме с 2 циклами
            //ssq event_period гггг/мм/дд чч:мм:сс гггг/мм/дд чч:мм:сс -- период эвента (при ручном задании)
            //ssq seal_period гггг/мм/дд чч:мм:сс гггг/мм/дд чч:мм:сс -- период действия печати (при ручном задании)
            //ssq quickcycle [время] -- быстрый цикл на [время] секунд
            //ssq setwinner [сторона] -- задает сторону-победитля
            //ssq sealowner [печать] [сторона] -- отдает печать стороне
        }
    }
}
