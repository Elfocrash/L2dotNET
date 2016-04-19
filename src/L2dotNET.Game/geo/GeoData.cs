using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using L2dotNET.Game.logger;

namespace L2dotNET.Game.geo
{
    public class GeoData : Geo
    {
        private static GeoData instance = new GeoData();
        public static GeoData getInstance()
        {
            return instance;
        }

        public static int MAP_MIN_X = -294912;//-163840;
        public static int MAP_MAX_X = 229375;
        public static int MAP_MIN_Y = -262144;
        public static int MAP_MAX_Y = 327679;
        public static int MAP_MIN_Z = -32768;
        public static int MAP_MAX_Z = 32767;

        private static byte EAST = 1, WEST = 2, SOUTH = 4, NORTH = 8, NSWE_ALL = 15, NSWE_NONE = 0;
        public const byte BLOCKTYPE_FLAT = 0;
        public const byte BLOCKTYPE_COMPLEX = 1;
        public const byte BLOCKTYPE_MULTILEVEL = 2;

        public static int GEODATA_SIZE_X = 26;//MAP_MAX_X - MAP_MIN_X + 1 >> 15;
        public static int GEODATA_SIZE_Y = 26;//MAP_MAX_Y - MAP_MIN_Y + 1 >> 15;

        public static byte GEODATA_ARRAY_OFFSET_X = 11;
        public static byte GEODATA_ARRAY_OFFSET_Y = 10;
        public static int BLOCKS_IN_MAP = 256 * 256;
        public static int MAX_LAYERS = 1; // меньше 1 быть не должно, что бы создавались временные массивы как минимум short[2]


        public byte[][][][] geodata = new byte[26][][][];


        public GeoData()
        {
            for (int a = 0; a < 26; a++)
                geodata[a] = new byte[26][][];

            loadGeo();
        }

        public override void loadGeo()
        {
            CLogger.extra_info("Loading geo. 26x26 mapworld");
            foreach (string fname in Directory.GetFiles(@"geo\", "*.l2j"))
            {
                FileInfo info = new FileInfo(fname);

                string[] stx = info.Name.Split('.')[0].Split('_');
                byte rxb = byte.Parse(stx[0]), ryb = byte.Parse(stx[1]);
                CLogger.extra_info("Loading " + fname + ". " + rxb + "_" + ryb);
                if (info.Length < 196608)
                {
                    CLogger.error("Unsupported geo file. L2J file cannot be smaller than 196kb. " + fname);
                    continue;
                }

                byte[][] blocks = new byte[65536][]; // 256 * 256

                int index = 0;
                byte[] data = File.ReadAllBytes(fname);
                for (int block = 0, n = blocks.Length; block < n; block++)
                {
                    byte type = data[index];
                    index++;
                    byte[] geoBlock;

                    switch (type)
                    {
                        case BLOCKTYPE_FLAT:
                            // Создаем блок геодаты
                            geoBlock = new byte[2 + 1];
                            // Читаем нужные даные с геодаты
                            geoBlock[0] = type;
                            geoBlock[1] = data[index];
                            geoBlock[2] = data[index + 1];
                            // Добавляем блок геодаты
                            blocks[block] = geoBlock;
                            index += 2;
                            break;
                        case BLOCKTYPE_COMPLEX:
                            // Создаем блок геодаты
                            geoBlock = new byte[128 + 1];

                            // Читаем даные с геодаты
                            geoBlock[0] = type;
                            byte sx = 1;
                            for (int a = index; a < index + 128; a++)
                                geoBlock[sx] = data[a];
                            // Увеличиваем индекс
                            index += 128;
                            // Добавляем блок геодаты
                            blocks[block] = geoBlock;
                            break;
                        case BLOCKTYPE_MULTILEVEL:
                            // Оригинальный индекс
                            int orgIndex = index;
                            // Считаем длинну блока геодаты
                            for (int b = 0; b < 64; b++)
                            {
                                byte layers = data[index];
                                MAX_LAYERS = Math.Max(MAX_LAYERS, layers);
                                index += (layers << 1) + 1;
                            }
                            // Получаем длинну
                            int diff = index - orgIndex;
                            // Создаем массив геодаты
                            geoBlock = new byte[diff + 1];
                            // Читаем даные с геодаты
                            geoBlock[0] = type;
                            int sp = 1;
                            for (int a = index; a < index + diff; a++)
                                geoBlock[sp] = data[a];
                            // Добавляем блок геодаты
                            blocks[block] = geoBlock;
                            break;
                        default:
                            CLogger.error("error parsing type " + type);
                            break;
                    }
                }
                try
                {
                    geodata[rxb][ryb] = blocks;
                }
                catch (Exception)
                {

                    Console.WriteLine("cant fit geo region");
                }
                // geo[rxb][ryb] = blocks;
                // geob.Add(this.hashId(rxb, ryb), blocks);
                CLogger.extra_info("Loaded " + info.Name + " " + (info.Length / 1024) + "kb and applied to '" + rxb + "_" + ryb + "'");

            }

        }

        public short getType(int x, int y)
        {
            return NgetType(x - MAP_MIN_X >> 4, y - MAP_MIN_Y >> 4);
        }

        public int getHeight(Location loc)
        {
            return getHeight(loc.getX(), loc.getY(), loc.getZ());
        }

        public int getHeight(int x, int y, int z)
        {
            return NgetHeight(x - MAP_MIN_X >> 4, y - MAP_MIN_Y >> 4, z);
        }

        public bool canMoveToCoord(int x, int y, int z, int tx, int ty, int tz)
        {
            return canMove(x, y, z, tx, ty, tz, false) == 0;
        }

        public short getNSWE(int x, int y, int z)
        {
            return NgetNSWE(x - MAP_MIN_X >> 4, y - MAP_MIN_Y >> 4, z);
        }

        public Location moveCheck(int x, int y, int z, int tx, int ty)
        {
            return moveCheck(x, y, z, tx, ty, false, false, false);
        }

        public Location moveCheck(int x, int y, int z, int tx, int ty, bool returnPrev)
        {
            return moveCheck(x, y, z, tx, ty, false, false, returnPrev);
        }

        public Location moveCheckWithCollision(int x, int y, int z, int tx, int ty)
        {
            return moveCheck(x, y, z, tx, ty, true, false, false);
        }

        public Location moveCheckWithCollision(int x, int y, int z, int tx, int ty, bool returnPrev)
        {
            return moveCheck(x, y, z, tx, ty, true, false, returnPrev);
        }

        public Location moveCheckBackward(int x, int y, int z, int tx, int ty)
        {
            return moveCheck(x, y, z, tx, ty, false, true, false);
        }

        public Location moveCheckBackward(int x, int y, int z, int tx, int ty, bool returnPrev)
        {
            return moveCheck(x, y, z, tx, ty, false, true, returnPrev);
        }

        public Location moveCheckBackwardWithCollision(int x, int y, int z, int tx, int ty)
        {
            return moveCheck(x, y, z, tx, ty, true, true, false);
        }

        public Location moveCheckBackwardWithCollision(int x, int y, int z, int tx, int ty, bool returnPrev)
        {
            return moveCheck(x, y, z, tx, ty, true, true, returnPrev);
        }

        public Location moveInWaterCheck(int x, int y, int z, int tx, int ty, int tz)
        {
            return MoveInWaterCheck(x - MAP_MIN_X >> 4, y - MAP_MIN_Y >> 4, z, tx - MAP_MIN_X >> 4, ty - MAP_MIN_Y >> 4, tz);
        }

        public Location moveCheckForAI(Location loc1, Location loc2)
        {
            return moveCheckForAI(loc1.getX() - MAP_MIN_X >> 4, loc1.getY() - MAP_MIN_Y >> 4, loc1.getZ(), loc2.getX() - MAP_MIN_X >> 4, loc2.getY() - MAP_MIN_Y >> 4);
        }

        public bool canSeeCoord(int x, int y, int z, int tx, int ty, int tz)
        {
            int mx = x - MAP_MIN_X >> 4;
            int my = y - MAP_MIN_Y >> 4;
            int tmx = tx - MAP_MIN_X >> 4;
            int tmy = ty - MAP_MIN_Y >> 4;
            return canSee(mx, my, z, tmx, tmy, tz).equals(tmx, tmy, tz) && canSee(tmx, tmy, tz, mx, my, z).equals(mx, my, z);
        }

        public bool canSeeCoord(L2Player actor, int tx, int ty, int tz, bool debug)
        {
            return actor != null && canSeeCoord(actor.X, actor.Y, actor.Z + (int)actor._collHeight + 64, tx, ty, tz, debug);
        }

        public bool canSeeCoord(int x, int y, int z, int tx, int ty, int tz, bool debug)
        {
            int mx = x - MAP_MIN_X >> 4;
            int my = y - MAP_MIN_Y >> 4;
            int tmx = tx - MAP_MIN_X >> 4;
            int tmy = ty - MAP_MIN_Y >> 4;
            Location result = canSee(mx, my, z, tmx, tmy, tz);
            bool ret = result.equals(tmx, tmy, tz);
            if (debug)
                Console.WriteLine("cantSee " + ret + " [" + result.h + "]: " + x + "," + y + "," + z + "->" + tx + "," + ty + "," + tz + " | " + result.toXYZString() + " =? " + tmx + "," + tmx + "," + tz);
            return ret;
        }

        public bool checkNSWE(byte NSWE, int x, int y, int tx, int ty)
        {
            if (NSWE == NSWE_ALL)
                return true;
            if (NSWE == NSWE_NONE)
                return false;
            if (tx > x)
            {
                if ((NSWE & EAST) == 0)
                    return false;
            }
            else if (tx < x)
            {
                if ((NSWE & WEST) == 0)
                    return false;
            }
            if (ty > y)
            {
                if ((NSWE & SOUTH) == 0)
                    return false;
            }
            else if (ty < y)
            {
                if ((NSWE & NORTH) == 0)
                    return false;
            }
            return true;
        }

        private bool NLOS_WATER(int x, int y, int z, int next_x, int next_y, int next_z)
        {
            Layer[] layers1 = NGetLayers(x, y);
            Layer[] layers2 = NGetLayers(next_x, next_y);
            if (layers1.Length == 0 || layers2.Length == 0)
                return true;

            // Находим ближайший к целевой клетке слой
            short z2 = short.MinValue;
            foreach (Layer layer in layers2)
                if (Math.Abs(next_z - z2) > Math.Abs(next_z - layer.height))
                    z2 = layer.height;

            // Луч проходит над преградой
            if (next_z + 32 >= z2)
                return true;

            // Либо перед нами стена, либо над нами потолок. Ищем слой пониже, для уточнения
            short z3 = short.MinValue;
            foreach (Layer layer in layers2)
                if (layer.height < z2 + 64 && Math.Abs(next_z - z3) > Math.Abs(next_z - layer.height))
                    z3 = layer.height;

            // Ниже нет слоев, значит это стена
            if (z3 == short.MinValue)
                return false;

            // Собираем данные о предыдущей клетке, игнорируя верхние слои
            short z1 = short.MinValue;
            byte NSWE1 = NSWE_ALL;
            foreach (Layer layer in layers1)
                if (layer.height < z + 64 && Math.Abs(z - z1) > Math.Abs(z - layer.height))
                {
                    z1 = layer.height;
                    NSWE1 = (byte)layer.nswe; //KID as byte
                }

            // Невозможная ситуация, но все же...
            if (z1 < -30000)
                return true;

            // Если есть NSWE, то считаем за стену
            return checkNSWE(NSWE1, x, y, next_x, next_y);
        }

        private static int findNearestLowerLayer(short[] layers, int z)
        {
            short h, nearest_layer_h = short.MinValue;
            int nearest_layer = int.MinValue;
            for (int i = 1; i <= layers[0]; i++)
            {
                h = (short)((short)(layers[i] & 0x0fff0) >> 1);
                if (h < z && nearest_layer_h < h)
                {
                    nearest_layer_h = h;
                    nearest_layer = layers[i];
                }
            }
            return nearest_layer;
        }

        private static short checkNoOneLayerInRangeAndFindNearestLowerLayer(short[] layers, int z0, int z1)
        {
            int z_min, z_max;
            if (z0 > z1)
            {
                z_min = z1;
                z_max = z0;
            }
            else
            {
                z_min = z0;
                z_max = z1;
            }
            short h, nearest_layer = short.MinValue, nearest_layer_h = short.MinValue;
            for (int i = 1; i <= layers[0]; i++)
            {
                h = (short)((short)(layers[i] & 0x0fff0) >> 1);
                if (z_min <= h && h <= z_max)
                    return short.MinValue;
                if (h < z0 && nearest_layer_h < h)
                {
                    nearest_layer_h = h;
                    nearest_layer = layers[i];
                }
            }
            return nearest_layer;
        }

        public static bool canSeeWallCheck(Layer layer, Layer nearest_lower_neighbor, byte directionNSWE)
        {
            return (layer.nswe & directionNSWE) != 0 || layer.height <= nearest_lower_neighbor.height || Math.Abs(layer.height - nearest_lower_neighbor.height) < 64;
        }

        public static bool canSeeWallCheck(short layer, short nearest_lower_neighbor, byte directionNSWE)
        {
            short layerh = (short)((short)(layer & 0x0fff0) >> 1);
            short nearest_lower_neighborh = (short)((short)(nearest_lower_neighbor & 0x0fff0) >> 1);
            int zdiff = nearest_lower_neighborh - layerh;
            return ((layer & 0x0F) & directionNSWE) != 0 || (zdiff > -64 /*&& zdiff != 0*/); // пофигу все плоские стенки и заборчики
        }

        public Location canSee(int _x, int _y, int _z, int _tx, int _ty, int _tz)
        {
            int diff_x = _tx - _x, diff_y = _ty - _y, diff_z = _tz - _z;
            int dx = Math.Abs(diff_x), dy = Math.Abs(diff_y);

            float steps = Math.Max(dx, dy);
            int curr_x = _x, curr_y = _y, curr_z = _z;
            short[] curr_layers = new short[MAX_LAYERS + 1];
            NGetLayers(curr_x, curr_y, curr_layers);

            Location result = new Location(_x, _y, _z, -1);

            if (steps == 0)
            {
                if (checkNoOneLayerInRangeAndFindNearestLowerLayer(curr_layers, curr_z, curr_z + diff_z) != short.MinValue)
                    result.set(_tx, _ty, _tz, 1);
                return result;
            }

            float step_x = diff_x / steps, step_y = diff_y / steps, step_z = diff_z / steps;
            int half_step_z = (int)(step_z / 2);
            float next_x = curr_x, next_y = curr_y, next_z = curr_z;
            int i_next_x, i_next_y, i_next_z, middle_z;
            short[] tmp_layers = new short[MAX_LAYERS + 1];
            short src_nearest_lower_layer, dst_nearest_lower_layer, tmp_nearest_lower_layer;

            for (int i = 0; i < steps; i++)
            {
                if (curr_layers[0] == 0)
                {
                    result.set(_tx, _ty, _tz, 0);
                    return result; // Здесь нет геодаты, разрешаем
                }

                next_x += step_x;
                next_y += step_y;
                next_z += step_z;
                i_next_x = (int)(next_x + 0.5f);
                i_next_y = (int)(next_y + 0.5f);
                i_next_z = (int)(next_z + 0.5f);
                middle_z = curr_z + half_step_z;

                if ((src_nearest_lower_layer = checkNoOneLayerInRangeAndFindNearestLowerLayer(curr_layers, curr_z, middle_z)) == short.MinValue)
                    return result.setH(-10); // либо есть преграждающая поверхность, либо нет снизу слоя и значит это "пустота", то что за стеной или за колоной

                NGetLayers(i_next_x, i_next_y, curr_layers);
                if (curr_layers[0] == 0)
                {
                    result.set(_tx, _ty, _tz, 0);
                    return result; // Здесь нет геодаты, разрешаем
                }

                if ((dst_nearest_lower_layer = checkNoOneLayerInRangeAndFindNearestLowerLayer(curr_layers, i_next_z, middle_z)) == short.MinValue)
                    return result.setH(-11); // либо есть преграда, либо нет снизу слоя и значит это "пустота", то что за стеной или за колоной

                if (curr_x == i_next_x)
                {
                    //движемся по вертикали
                    if (!canSeeWallCheck(src_nearest_lower_layer, dst_nearest_lower_layer, i_next_y > curr_y ? SOUTH : NORTH))
                        return result.setH(-20);
                }
                else if (curr_y == i_next_y)
                {
                    //движемся по горизонтали
                    if (!canSeeWallCheck(src_nearest_lower_layer, dst_nearest_lower_layer, i_next_x > curr_x ? EAST : WEST))
                        return result.setH(-21);
                }
                else
                {
                    //движемся по диагонали
                    NGetLayers(curr_x, i_next_y, tmp_layers);
                    if (tmp_layers[0] == 0)
                    {
                        result.set(_tx, _ty, _tz, 0);
                        return result; // Здесь нет геодаты, разрешаем
                    }
                    if ((tmp_nearest_lower_layer = checkNoOneLayerInRangeAndFindNearestLowerLayer(tmp_layers, i_next_z, middle_z)) == short.MinValue)
                        return result.setH(-30); // либо есть преграда, либо нет снизу слоя и значит это "пустота", то что за стеной или за колоной

                    if (!(canSeeWallCheck(src_nearest_lower_layer, tmp_nearest_lower_layer, i_next_y > curr_y ? SOUTH : NORTH) && canSeeWallCheck(tmp_nearest_lower_layer, dst_nearest_lower_layer, i_next_x > curr_x ? EAST : WEST)))
                    {
                        NGetLayers(i_next_x, curr_y, tmp_layers);
                        if (tmp_layers[0] == 0)
                        {
                            result.set(_tx, _ty, _tz, 0);
                            return result; // Здесь нет геодаты, разрешаем
                        }
                        if ((tmp_nearest_lower_layer = checkNoOneLayerInRangeAndFindNearestLowerLayer(tmp_layers, i_next_z, middle_z)) == short.MinValue)
                            return result.setH(-31); // либо есть преграда, либо нет снизу слоя и значит это "пустота", то что за стеной или за колоной
                        if (!canSeeWallCheck(src_nearest_lower_layer, tmp_nearest_lower_layer, i_next_x > curr_x ? EAST : WEST))
                            return result.setH(-32);
                        if (!canSeeWallCheck(tmp_nearest_lower_layer, dst_nearest_lower_layer, i_next_y > curr_y ? SOUTH : NORTH))
                            return result.setH(-33);
                    }
                }

                result.set(curr_x, curr_y, curr_z);
                curr_x = i_next_x;
                curr_y = i_next_y;
                curr_z = i_next_z;
            }

            result.set(_tx, _ty, _tz, 0xFF);
            return result;
        }

        private Location MoveInWaterCheck(int x, int y, int z, int tx, int ty, int tz)
        {
            int dx = tx - x;
            int dy = ty - y;
            int dz = tz - z;
            int inc_x = sign(dx);
            int inc_y = sign(dy);
            dx = Math.Abs(dx);
            dy = Math.Abs(dy);
            if (dx + dy == 0)
                return new Location(x, y, z).geo2world();
            float inc_z_for_x = dx == 0 ? 0 : dz / dx;
            float inc_z_for_y = dy == 0 ? 0 : dz / dy;
            int prev_x;
            int prev_y;
            int prev_z;
            int next_x = x;
            int next_y = y;
            int next_z = z;
            if (dx >= dy) // dy/dx <= 1
            {
                int delta_A = 2 * dy;
                int d = delta_A - dx;
                int delta_B = delta_A - 2 * dx;
                for (int i = 0; i < dx; i++)
                {
                    prev_x = x;
                    prev_y = y;
                    prev_z = z;
                    x = next_x;
                    y = next_y;
                    z = next_z;
                    if (d > 0)
                    {
                        d += delta_B;
                        next_x += inc_x;
                        next_z += (int)inc_z_for_x;//kid
                        next_y += inc_y;
                        next_z += (int)inc_z_for_y;//kid
                    }
                    else
                    {
                        d += delta_A;
                        next_x += inc_x;
                        next_z += (int)inc_z_for_x;//kid
                    }
                    if (!NLOS_WATER(x, y, z, next_x, next_y, next_z))
                        return new Location(prev_x, prev_y, prev_z).geo2world();
                }
            }
            else
            {
                int delta_A = 2 * dx;
                int d = delta_A - dy;
                int delta_B = delta_A - 2 * dy;
                for (int i = 0; i < dy; i++)
                {
                    prev_x = x;
                    prev_y = y;
                    prev_z = z;
                    x = next_x;
                    y = next_y;
                    z = next_z;
                    if (d > 0)
                    {
                        d += delta_B;
                        next_x += inc_x;
                        next_z += (int)inc_z_for_x;//kid
                        next_y += inc_y;
                        next_z += (int)inc_z_for_y;//kid
                    }
                    else
                    {
                        d += delta_A;
                        next_y += inc_y;
                        next_z += (int)inc_z_for_y;//kid
                    }
                    if (!NLOS_WATER(x, y, z, next_x, next_y, next_z))
                        return new Location(prev_x, prev_y, prev_z).geo2world();
                }
            }
            return new Location(next_x, next_y, next_z).geo2world();
        }

        public int canMove(int __x, int __y, int _z, int __tx, int __ty, int _tz, bool withCollision)
        {
            int _x = __x - MAP_MIN_X >> 4;
            int _y = __y - MAP_MIN_Y >> 4;
            int _tx = __tx - MAP_MIN_X >> 4;
            int _ty = __ty - MAP_MIN_Y >> 4;
            int diff_x = _tx - _x, diff_y = _ty - _y, diff_z = _tz - _z;
            int dx = Math.Abs(diff_x), dy = Math.Abs(diff_y), dz = Math.Abs(diff_z);
            float steps = Math.Max(dx, dy);

            int curr_x = _x, curr_y = _y, curr_z = _z;
            short[] curr_layers = new short[MAX_LAYERS + 1];
            NGetLayers(curr_x, curr_y, curr_layers);
            if (curr_layers[0] == 0)
                return 0;

            if (steps == 0)
                return -5;

            float step_x = diff_x / steps, step_y = diff_y / steps;
            float next_x = curr_x, next_y = curr_y;
            int i_next_x, i_next_y;

            short[] next_layers = new short[MAX_LAYERS + 1];
            short[] temp_layers = new short[MAX_LAYERS + 1];
            short[] curr_next_switcher;

            for (int i = 0; i < steps; i++)
            {
                next_x += step_x;
                i_next_x = (int)(next_x + 0.5f);
                i_next_y = (int)(next_y + 0.5f);
                NGetLayers(i_next_x, i_next_y, next_layers);
                if ((curr_z = NcanMoveNext(curr_x, curr_y, curr_z, curr_layers, i_next_x, i_next_y, next_layers, temp_layers, withCollision)) == int.MinValue)
                    return 1;
                next_y += step_y;
                i_next_y = (int)(next_y + 0.5f);
                if ((curr_z = NcanMoveNext(i_next_x, curr_y, curr_z, curr_layers, i_next_x, i_next_y, next_layers, temp_layers, withCollision)) == int.MinValue)
                    return 1;
                curr_next_switcher = curr_layers;
                curr_layers = next_layers;
                next_layers = curr_next_switcher;
                curr_x = i_next_x;
                curr_y = i_next_y;
            }
            diff_z = curr_z - _tz;
            dz = Math.Abs(diff_z);
            // if (GeoConfig.ALLOW_FALL_FROM_WALLS) KID
            //    return diff_z < 64 ? 0 : diff_z * 10000;
            return dz > 64 ? dz * 1000 : 0;
        }

        public Location moveCheck(int __x, int __y, int _z, int __tx, int __ty, bool withCollision, bool backwardMove, bool returnPrev)
        {
            int _x = __x - MAP_MIN_X >> 4;
            int _y = __y - MAP_MIN_Y >> 4;
            int _tx = __tx - MAP_MIN_X >> 4;
            int _ty = __ty - MAP_MIN_Y >> 4;

            int diff_x = _tx - _x, diff_y = _ty - _y;
            int dx = Math.Abs(diff_x), dy = Math.Abs(diff_y);
            float steps = Math.Max(dx, dy);
            if (steps == 0)
                return new Location(__x, __y, _z);

            float step_x = diff_x / steps, step_y = diff_y / steps;
            int curr_x = _x, curr_y = _y, curr_z = _z;
            float next_x = curr_x, next_y = curr_y;
            int i_next_x, i_next_y, i_next_z;

            short[] next_layers = new short[MAX_LAYERS + 1];
            short[] temp_layers = new short[MAX_LAYERS + 1];
            short[] curr_layers = new short[MAX_LAYERS + 1];
            short[] curr_next_switcher;
            NGetLayers(curr_x, curr_y, curr_layers);
            int prev_x = curr_x, prev_y = curr_y, prev_z = curr_z;

            for (int i = 0; i < steps; i++)
            {
                next_x += step_x;
                next_y += step_y;
                i_next_x = (int)(next_x + 0.5f);
                i_next_y = (int)(next_y + 0.5f);
                NGetLayers(i_next_x, i_next_y, next_layers);
                if (backwardMove)
                {
                    if ((i_next_z = NcanMoveNext(i_next_x, i_next_y, curr_z, next_layers, curr_x, curr_y, curr_layers, temp_layers, withCollision)) == int.MinValue)
                        break;
                }
                else if ((i_next_z = NcanMoveNext(curr_x, curr_y, curr_z, curr_layers, i_next_x, i_next_y, next_layers, temp_layers, withCollision)) == int.MinValue)
                    break;
                curr_next_switcher = curr_layers;
                curr_layers = next_layers;
                next_layers = curr_next_switcher;
                if (returnPrev)
                {
                    prev_x = curr_x;
                    prev_y = curr_y;
                    prev_z = curr_z;
                }
                curr_x = i_next_x;
                curr_y = i_next_y;
                curr_z = i_next_z;
            }

            if (returnPrev)
            {
                curr_x = prev_x;
                curr_y = prev_y;
                curr_z = prev_z;
            }

            if (curr_x == _x && curr_y == _y)
                return new Location(__x, __y, _z);
            //log.info("move" + (backwardMove ? " back" : "") + (withCollision ? " +collision" : "") + ": " + curr_x + " " + curr_y + " " + curr_z + " / xyz: " + __x + " " + __y + " " + _z + " / to xy: " + __tx + " " + __ty + " / geo xy: " + _x + " " + _y + " / geo to xy: " + _tx + " " + _ty);
            return new Location(curr_x, curr_y, curr_z).geo2world();
        }

        private Location moveCheckForAI(int x, int y, int z, int tx, int ty)
        {
            int dx = tx - x;
            int dy = ty - y;
            int inc_x = sign(dx);
            int inc_y = sign(dy);
            dx = Math.Abs(dx);
            dy = Math.Abs(dy);
            if (dx + dy < 2 || dx == 2 && dy == 0 || dx == 0 && dy == 2)
                return new Location(x, y, z).geo2world();
            int prev_x = x;
            int prev_y = y;
            int prev_z = z;
            int next_x = x;
            int next_y = y;
            int next_z = z;
            if (dx >= dy) // dy/dx <= 1
            {
                int delta_A = 2 * dy;
                int d = delta_A - dx;
                int delta_B = delta_A - 2 * dx;
                for (int i = 0; i < dx; i++)
                {
                    prev_x = x;
                    prev_y = y;
                    prev_z = z;
                    x = next_x;
                    y = next_y;
                    z = next_z;
                    if (d > 0)
                    {
                        d += delta_B;
                        next_x += inc_x;
                        next_z = NcanMoveNextForAI(x, y, z, next_x, next_y);
                        if (next_z == 0)
                            return new Location(prev_x, prev_y, prev_z).geo2world();
                        next_y += inc_y;
                    }
                    else
                    {
                        d += delta_A;
                        next_x += inc_x;
                    }
                    next_z = NcanMoveNextForAI(x, y, z, next_x, next_y);
                    if (next_z == 0)
                        return new Location(prev_x, prev_y, prev_z).geo2world();
                }
            }
            else
            {
                int delta_A = 2 * dx;
                int d = delta_A - dy;
                int delta_B = delta_A - 2 * dy;
                for (int i = 0; i < dy; i++)
                {
                    prev_x = x;
                    prev_y = y;
                    prev_z = z;
                    x = next_x;
                    y = next_y;
                    z = next_z;
                    if (d > 0)
                    {
                        d += delta_B;
                        next_x += inc_x;
                        next_z = NcanMoveNextForAI(x, y, z, next_x, next_y);
                        if (next_z == 0)
                            return new Location(prev_x, prev_y, prev_z).geo2world();
                        next_y += inc_y;
                    }
                    else
                    {
                        d += delta_A;
                        next_y += inc_y;
                    }
                    next_z = NcanMoveNextForAI(x, y, z, next_x, next_y);
                    if (next_z == 0)
                        return new Location(prev_x, prev_y, prev_z).geo2world();
                }
            }
            return new Location(next_x, next_y, next_z).geo2world();
        }

        private bool NcanMoveNextExCheck(int x, int y, int h, int nextx, int nexty, int hexth, short[] temp_layers)
        {
            NGetLayers(x, y, temp_layers);
            if (temp_layers[0] == 0)
                return true;

            int temp_layer;
            if ((temp_layer = findNearestLowerLayer(temp_layers, h + 64)) == int.MinValue)
                return false;
            short temp_layer_h = (short)((short)(temp_layer & 0x0fff0) >> 1);
            if (Math.Abs(temp_layer_h - hexth) >= 64 || Math.Abs(temp_layer_h - h) >= 64)
                return false;
            return checkNSWE((byte)(temp_layer & 0x0F), x, y, nextx, nexty);
        }

        public int NcanMoveNext(int x, int y, int z, short[] layers, int next_x, int next_y, short[] next_layers, short[] temp_layers, bool withCollision)
        {
            if (layers[0] == 0 || next_layers[0] == 0)
                return z;

            int layer, next_layer;
            if ((layer = findNearestLowerLayer(layers, z + 64)) == int.MinValue)
                return int.MinValue;

            byte layer_nswe = (byte)(layer & 0x0F);
            if (!checkNSWE(layer_nswe, x, y, next_x, next_y))
                return int.MinValue;

            short layer_h = (short)(((short)(layer & 0x0fff0) >> 1));
            if ((next_layer = findNearestLowerLayer(next_layers, layer_h + 64)) == int.MinValue)
                return int.MinValue;

            short next_layer_h = (short)((short)(next_layer & 0x0fff0) >> 1);
            /*if(withCollision && next_layer_h + Config.MAX_Z_DIFF < layer_h)
                return int.MinValue;*/

            // если движение не по диагонали
            if (x == next_x || y == next_y)
            {
                if (withCollision)
                {
                    //short[] heightNSWE = temp_layers;
                    if (x == next_x)
                    {
                        NgetHeightAndNSWE(x - 1, y, layer_h, temp_layers);
                        if (Math.Abs(temp_layers[0] - layer_h) > 15 || !checkNSWE(layer_nswe, x - 1, y, x, y) || !checkNSWE((byte)temp_layers[1], x - 1, y, x - 1, next_y))
                            return int.MinValue;

                        NgetHeightAndNSWE(x + 1, y, layer_h, temp_layers);
                        if (Math.Abs(temp_layers[0] - layer_h) > 15 || !checkNSWE(layer_nswe, x + 1, y, x, y) || !checkNSWE((byte)temp_layers[1], x + 1, y, x + 1, next_y))
                            return int.MinValue;

                        return next_layer_h;
                    }

                    NgetHeightAndNSWE(x, y - 1, layer_h, temp_layers);
                    if (Math.Abs(temp_layers[0] - layer_h) >= 64 || !checkNSWE(layer_nswe, x, y - 1, x, y) || !checkNSWE((byte)temp_layers[1], x, y - 1, next_x, y - 1))
                        return int.MinValue;

                    NgetHeightAndNSWE(x, y + 1, layer_h, temp_layers);
                    if (Math.Abs(temp_layers[0] - layer_h) >= 64 || !checkNSWE(layer_nswe, x, y + 1, x, y) || !checkNSWE((byte)temp_layers[1], x, y + 1, next_x, y + 1))
                        return int.MinValue;
                }

                return next_layer_h;
            }

            if (!NcanMoveNextExCheck(x, next_y, layer_h, next_x, next_y, next_layer_h, temp_layers))
                return int.MinValue;
            if (!NcanMoveNextExCheck(next_x, y, layer_h, next_x, next_y, next_layer_h, temp_layers))
                return int.MinValue;

            //FIXME if(withCollision)

            return next_layer_h;
        }

        public int NcanMoveNextForAI(int x, int y, int z, int next_x, int next_y)
        {
            Layer[] layers1 = NGetLayers(x, y);
            Layer[] layers2 = NGetLayers(next_x, next_y);

            if (layers1.Length == 0 || layers2.Length == 0)
                return z == 0 ? 1 : z;

            short z1 = short.MinValue;
            short z2 = short.MinValue;
            byte NSWE1 = NSWE_ALL;
            byte NSWE2 = NSWE_ALL;

            foreach (Layer layer in layers1)
                if (layer.height < z + 64 && Math.Abs(z - z1) > Math.Abs(z - layer.height))
                {
                    z1 = layer.height;
                    NSWE1 = (byte)layer.nswe;
                }

            // Вторая попытка с более мягкими условиями
            if (z1 < -30000)
                foreach (Layer layer in layers1)
                    if (Math.Abs(z - z1) > Math.Abs(z - layer.height))
                    {
                        z1 = layer.height;
                        NSWE1 = (byte)layer.nswe;
                    }

            if (z1 < -30000)
                return 0;

            foreach (Layer layer in layers2)
                if (layer.height < z1 + 64 && Math.Abs(z1 - z2) > Math.Abs(z1 - layer.height))
                {
                    z2 = layer.height;
                    NSWE2 = (byte)layer.nswe;
                }

            // Вторая попытка с более мягкими условиями
            if (z2 < -30000)
                foreach (Layer layer in layers2)
                    if (Math.Abs(z1 - z2) > Math.Abs(z1 - layer.height))
                    {
                        z2 = layer.height;
                        NSWE2 = (byte)layer.nswe;
                    }

            if (z2 < -30000)
                return 0;

            //if(z1 > z2 && z1 - z2 > GeoConfig.MAX_Z_DIFF) KID
            //	return 0;

            if (!checkNSWE(NSWE1, x, y, next_x, next_y) || !checkNSWE(NSWE2, next_x, next_y, x, y))
                return 0;

            return z2 == 0 ? 1 : z2;
        }

        public void NGetLayers(int geoX, int geoY, short[] result)
        {
            result[0] = 0;
            byte[] block = getGeoBlockFromGeoCoords(geoX, geoY);
            if (block == null)
                return;

            int cellX, cellY;
            int index = 0;
            // Read current block type: 0 - flat, 1 - complex, 2 - multilevel
            byte type = block[index];
            index++;

            switch (type)
            {
                case BLOCKTYPE_FLAT:
                    short height = makeShort(block[index + 1], block[index]);
                    height = (short)(height & 0x0fff0);
                    result[0]++;
                    result[1] = (short)((short)(height << 1) | NSWE_ALL);
                    return;
                case BLOCKTYPE_COMPLEX:
                    cellX = getCell(geoX);
                    cellY = getCell(geoY);
                    index += (cellX << 3) + cellY << 1;
                    height = makeShort(block[index + 1], block[index]);
                    result[0]++;
                    result[1] = height;
                    return;
                case BLOCKTYPE_MULTILEVEL:
                    cellX = getCell(geoX);
                    cellY = getCell(geoY);
                    int offset = (cellX << 3) + cellY;
                    while (offset > 0)
                    {
                        byte lc = block[index];
                        index += (lc << 1) + 1;
                        offset--;
                    }
                    byte layer_count = block[index];
                    index++;
                    if (layer_count <= 0 || layer_count > MAX_LAYERS)
                        return;
                    result[0] = layer_count;
                    while (layer_count > 0)
                    {
                        result[layer_count] = makeShort(block[index + 1], block[index]);
                        layer_count--;
                        index += 2;
                    }
                    return;
                default:
                    Console.WriteLine("GeoEngine: Unknown block type");
                    return;
            }
        }

        private Layer[] NGetLayers(int geoX, int geoY)
        {
            byte[] block = getGeoBlockFromGeoCoords(geoX, geoY);

            if (block == null)
                return new Layer[0];

            int cellX, cellY;
            int index = 0;
            // Read current block type: 0 - flat, 1 - complex, 2 - multilevel
            byte type = block[index];
            index++;

            switch (type)
            {
                case BLOCKTYPE_FLAT:
                    short height = makeShort(block[index + 1], block[index]);
                    height = (short)(height & 0x0fff0);
                    return new Layer[] { new Layer(height, NSWE_ALL) };
                case BLOCKTYPE_COMPLEX:
                    cellX = getCell(geoX);
                    cellY = getCell(geoY);
                    index += (cellX << 3) + cellY << 1;
                    height = makeShort(block[index + 1], block[index]);
                    return new Layer[] { new Layer((short)((short)(height & 0x0fff0) >> 1), (byte)(height & 0x0F)) };
                case BLOCKTYPE_MULTILEVEL:
                    cellX = getCell(geoX);
                    cellY = getCell(geoY);
                    int offset = (cellX << 3) + cellY;
                    while (offset > 0)
                    {
                        byte lc = block[index];
                        index += (lc << 1) + 1;
                        offset--;
                    }
                    byte layer_count = block[index];
                    index++;
                    if (layer_count <= 0 || layer_count > MAX_LAYERS)
                        return new Layer[0];
                    Layer[] layers = new Layer[layer_count];
                    while (layer_count > 0)
                    {
                        height = makeShort(block[index + 1], block[index]);
                        layer_count--;
                        layers[layer_count] = new Layer((short)((short)(height & 0x0fff0) >> 1), (byte)(height & 0x0F));
                        index += 2;
                    }
                    return layers;
                default:
                    Console.WriteLine("GeoEngine: Unknown block type");
                    return new Layer[0];
            }
        }

        private short NgetType(int geoX, int geoY)
        {
            byte[] block = getGeoBlockFromGeoCoords(geoX, geoY);

            if (block == null)
                return 0;

            return block[0];
        }

        public int NgetHeight(int geoX, int geoY, int z)
        {
            byte[] block = getGeoBlockFromGeoCoords(geoX, geoY);

            if (block == null)
                return z;

            int cellX, cellY, index = 0;

            // Read current block type: 0 - flat, 1 - complex, 2 - multilevel
            byte type = block[index];
            index++;

            short height;
            switch (type)
            {
                case BLOCKTYPE_FLAT:
                    height = makeShort(block[index + 1], block[index]);
                    return (short)(height & 0x0fff0);
                case BLOCKTYPE_COMPLEX:
                    cellX = getCell(geoX);
                    cellY = getCell(geoY);
                    index += (cellX << 3) + cellY << 1;
                    height = makeShort(block[index + 1], block[index]);
                    return (short)((short)(height & 0x0fff0) >> 1); // height / 2
                case BLOCKTYPE_MULTILEVEL:
                    cellX = getCell(geoX);
                    cellY = getCell(geoY);
                    int offset = (cellX << 3) + cellY;
                    while (offset > 0)
                    {
                        byte lc = block[index];
                        index += (lc << 1) + 1;
                        offset--;
                    }
                    byte layers = block[index];
                    index++;
                    if (layers <= 0 || layers > MAX_LAYERS)
                        return (short)z;
                    int z_nearest_lower_limit = z + 64;
                    int z_nearest_lower = int.MinValue;
                    int z_nearest = int.MinValue;

                    while (layers > 0)
                    {
                        height = (short)((short)(makeShort(block[index + 1], block[index]) & 0x0fff0) >> 1);
                        if (height < z_nearest_lower_limit)
                            z_nearest_lower = Math.Max(z_nearest_lower, height);
                        else if (Math.Abs(z - height) < Math.Abs(z - z_nearest))
                            z_nearest = height;
                        layers--;
                        index += 2;
                    }

                    return z_nearest_lower != int.MinValue ? z_nearest_lower : z_nearest;
                default:
                    Console.WriteLine("GeoEngine: Unknown blockType");
                    return z;
            }
        }

        public byte NgetNSWE(int geoX, int geoY, int z)
        {
            byte[] block = getGeoBlockFromGeoCoords(geoX, geoY);

            if (block == null)
                return NSWE_ALL;

            int cellX, cellY;
            int index = 0;

            // Read current block type: 0 - flat, 1 - complex, 2 - multilevel
            byte type = block[index];
            index++;

            switch (type)
            {
                case BLOCKTYPE_FLAT:
                    return NSWE_ALL;
                case BLOCKTYPE_COMPLEX:
                    cellX = getCell(geoX);
                    cellY = getCell(geoY);
                    index += (cellX << 3) + cellY << 1;
                    short height = makeShort(block[index + 1], block[index]);
                    return (byte)(height & 0x0F);
                case BLOCKTYPE_MULTILEVEL:
                    cellX = getCell(geoX);
                    cellY = getCell(geoY);
                    int offset = (cellX << 3) + cellY;
                    while (offset > 0)
                    {
                        byte lc = block[index];
                        index += (lc << 1) + 1;
                        offset--;
                    }
                    byte layers = block[index];
                    index++;
                    if (layers <= 0 || layers > MAX_LAYERS)
                        return NSWE_ALL;

                    short tempz1 = short.MinValue;
                    short tempz2 = short.MinValue;
                    int index_nswe1 = NSWE_NONE;
                    int index_nswe2 = NSWE_NONE;
                    int z_nearest_lower_limit = z + 64;

                    while (layers > 0)
                    {
                        height = (short)((short)(makeShort(block[index + 1], block[index]) & 0x0fff0) >> 1); // height / 2

                        if (height < z_nearest_lower_limit)
                        {
                            if (height > tempz1)
                            {
                                tempz1 = height;
                                index_nswe1 = index;
                            }
                        }
                        else if (Math.Abs(z - height) < Math.Abs(z - tempz2))
                        {
                            tempz2 = height;
                            index_nswe2 = index;
                        }

                        layers--;
                        index += 2;
                    }

                    if (index_nswe1 > 0)
                        return (byte)(makeShort(block[index_nswe1 + 1], block[index_nswe1]) & 0x0F);
                    if (index_nswe2 > 0)
                        return (byte)(makeShort(block[index_nswe2 + 1], block[index_nswe2]) & 0x0F);

                    return NSWE_ALL;
                default:
                    Console.WriteLine("GeoEngine: Unknown block type.");
                    return NSWE_ALL;
            }
        }

        public void NgetHeightAndNSWE(int geoX, int geoY, short z, short[] result)
        {
            byte[] block = getGeoBlockFromGeoCoords(geoX, geoY);

            if (block == null)
            {
                result[0] = z;
                result[1] = NSWE_ALL;
                return;
            }

            int cellX, cellY, index = 0;
            short height, nswe = NSWE_ALL;

            // Read current block type: 0 - flat, 1 - complex, 2 - multilevel
            byte type = block[index];
            index++;

            switch (type)
            {
                case BLOCKTYPE_FLAT:
                    height = makeShort(block[index + 1], block[index]);
                    result[0] = (short)(height & 0x0fff0);
                    result[1] = NSWE_ALL;
                    return;
                case BLOCKTYPE_COMPLEX:
                    cellX = getCell(geoX);
                    cellY = getCell(geoY);
                    index += (cellX << 3) + cellY << 1;
                    height = makeShort(block[index + 1], block[index]);
                    result[0] = (short)((short)(height & 0x0fff0) >> 1); // height / 2
                    result[1] = (short)(height & 0x0F);
                    return;
                case BLOCKTYPE_MULTILEVEL:
                    cellX = getCell(geoX);
                    cellY = getCell(geoY);
                    int offset = (cellX << 3) + cellY;
                    while (offset > 0)
                    {
                        byte lc = block[index];
                        index += (lc << 1) + 1;
                        offset--;
                    }
                    byte layers = block[index];
                    index++;
                    if (layers <= 0 || layers > MAX_LAYERS)
                    {
                        result[0] = z;
                        result[1] = NSWE_ALL;
                        return;
                    }

                    short tempz1 = short.MinValue;
                    short tempz2 = short.MinValue;
                    int index_nswe1 = 0;
                    int index_nswe2 = 0;
                    int z_nearest_lower_limit = z + 64;

                    while (layers > 0)
                    {
                        height = (short)((short)(makeShort(block[index + 1], block[index]) & 0x0fff0) >> 1); // height / 2

                        if (height < z_nearest_lower_limit)
                        {
                            if (height > tempz1)
                            {
                                tempz1 = height;
                                index_nswe1 = index;
                            }
                        }
                        else if (Math.Abs(z - height) < Math.Abs(z - tempz2))
                        {
                            tempz2 = height;
                            index_nswe2 = index;
                        }

                        layers--;
                        index += 2;
                    }

                    if (index_nswe1 > 0)
                    {
                        nswe = makeShort(block[index_nswe1 + 1], block[index_nswe1]);
                        nswe &= 0x0F;
                    }
                    else if (index_nswe2 > 0)
                    {
                        nswe = makeShort(block[index_nswe2 + 1], block[index_nswe2]);
                        nswe &= 0x0F;
                    }
                    result[0] = tempz1 > short.MinValue ? tempz1 : tempz2;
                    result[1] = nswe;
                    return;
            }

            Console.WriteLine("GeoEngine: Unknown block type.");
            result[0] = z;
            result[1] = 15;
        }

        protected static short makeShort(byte b1, byte b0)
        {
            return (short)(b1 << 8 | b0 & 0xff);
        }

        protected static int getBlock(int geoPos)
        {
            return (geoPos >> 3) % 256;
        }

        protected static int getCell(int geoPos)
        {
            return geoPos % 8;
        }

        protected static int getBlockIndex(int blockX, int blockY)
        {
            return (blockX << 8) + blockY;
        }

        private static short sign(int x)
        {
            if (x >= 0)
                return +1;
            return -1;
        }

        protected byte[] getGeoBlockFromGeoCoords(int geoX, int geoY)
        {
            int ix = geoX >> 11;
            int iy = geoY >> 11;
            ix += 10;
            iy += 10;

            if (ix < 0 || ix >= GEODATA_SIZE_X || iy < 0 || iy >= GEODATA_SIZE_Y)
                return null;

            byte[][] region = geodata[ix][iy];

            if (region == null)
            {
                Console.WriteLine("null region! "+ix+"_"+iy);
                return null;
            }

            int blockX = getBlock(geoX);
            int blockY = getBlock(geoY);

            return region[getBlockIndex(blockX, blockY)];
        }

        public static long makeLong(int nLo, int nHi)
        {
            return ((long)nHi << 32) | (nLo & 0x00000000ffffffffL);
        }
    }
}
