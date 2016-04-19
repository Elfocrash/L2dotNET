using System.Data;
using L2dotNET.Game.db;
using L2dotNET.Game.model.inventory;
using L2dotNET.Game.model.npcs;
using L2dotNET.Game.model.playable.petai;
using L2dotNET.Game.tables;
using MySql.Data.MySqlClient;

namespace L2dotNET.Game.model.playable
{
    public class L2Pet : L2Summon
    {
        public InvPet Inventory;
        public L2Pet()
        {
            ObjectSummonType = 2;
            ObjID = IdFactory.getInstance().nextId();
            Name = "";
            Inventory = new InvPet(this);
        }

        public override void onAction(L2Player player)
        {
            player.sendMessage(asString());

            player.ChangeTarget(this);
        }

        public override void setTemplate(NpcTemplate template)
        {
            Template = template;
            CStatsInit();
            CharacterStat.setTemplate(template);
            CurHP = CharacterStat.getStat(skills2.TEffectType.b_max_hp);
            MaxTime = 1200; //20 минут
            CurrentTime = MaxTime;
            Level = 35;
        }

        public override int MaxWeight()
        {
            return 300000;
        }

        public override void unSummon()
        {
            sql_update();
            base.unSummon();
        }

        public bool IsRestored = false;
        public void sql_update()
        {
            if (IsRestored)
            {
                SQL_Block sqb = new SQL_Block("user_pets");
                sqb.param("name", Name);
                sqb.param("lvl", Level);
                sqb.param("hp", CurHP);
                sqb.param("mp", CurMP);
                sqb.param("exp", StatusExp);
                sqb.param("sp", StatusSP);
                sqb.param("fed", CurrentTime);
                sqb.where("ownerId", Owner.ObjID);
                sqb.where("id", ControlItem.ObjID);
                sqb.sql_update(false);
            }
            else
            {
                SQL_Block sqb = new SQL_Block("user_pets");
                sqb.param("ownerId", Owner.ObjID);
                sqb.param("id", ControlItem.ObjID);
                sqb.param("name", Name);
                sqb.param("lvl", Level);
                sqb.param("hp", CurHP);
                sqb.param("mp", CurMP);
                sqb.param("exp", StatusExp);
                sqb.param("sp", StatusSP);
                sqb.param("fed", CurrentTime);
                sqb.sql_insert(false);
                IsRestored = true;
            }
        }

        public void sql_restore()
        {
            MySqlConnection connection = SQLjec.getInstance().conn();
            MySqlCommand cmd = connection.CreateCommand();

            connection.Open();

            cmd.CommandText = "SELECT * FROM user_pets WHERE ownerId=" + Owner.ObjID+" and id="+ControlItem.ObjID;
            cmd.CommandType = CommandType.Text;

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                IsRestored = true;
                Name = reader.GetString("name");
                Level = (byte)reader.GetInt16("lvl");
                CurHP = reader.GetInt32("hp");
                CurMP = reader.GetInt32("mp");
                StatusExp = reader.GetInt64("exp");
                StatusSP = reader.GetInt32("sp");
                CurrentTime = reader.GetInt32("fed");
            }

            reader.Close();
            connection.Close();

            if (!IsRestored)
                sql_update();
        }

        public void sql_delete()
        {
            SQL_Block sqb = new SQL_Block("user_pets");
            sqb.where("ownerId", Owner.ObjID);
            sqb.where("id", ControlItem.ObjID);
            sqb.sql_delete(false);
        }

        public override string asString()
        {
            return "L2Pet:" + ObjID + "";
        }
    }
}
