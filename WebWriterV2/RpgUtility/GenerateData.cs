using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Dao.Model;
//using WebWriterV2.Models.rpg;
using WebWriterV2.RpgUtility;

namespace WebWriterV2.RpgUtility
{
    public static class GenerateData
    {
        public const string Hp = "Hp";
        public const string MaxHp = "MaxHp";
        public const string Mp = "Mp";
        public const string MaxMp = "MaxMp";
        public const string Dodge = "Dodge";
        public const string Damage = "Damage";
        public const string Armor = "Armor";

        public const string Gold = "Gold";

        public const string Sword = "Меч";
        public const string ArmorBra = "Броне лифчик";
        public const string HealingPotion = "Лечебное зелье";

        public const string Strength = "Сила";
        public const string Agility = "Ловкость";
        public const string Charism = "Красота";

        public const string SchoolBaseSkillName = "Базовые умения";
        public const string SchoolColdSkillName = "Школа льда";
        public const string SchoolFireSkillName = "Школа пламени";
        public const string SchoolNiceSkillName = "Соблазнения";

        public static Guild GetGuild(List<Hero> heroes, List<SkillSchool> skillSchools)
        {
            var guild = new Guild
            {
                Name = "Пьяный Бобры",
                Desc = "Основанна стареющим, но некогда велики воином, трижды спасшим мир, два двыжды убившим саму смерть ну и так по мелочи подвигов",
                Heroes = heroes,
                Influence = 10,
                Gold = 954,
                TrainingRooms = GenerateTrainingRooms(skillSchools).ToList()//.Where(x => x.School.Name == "Школа льда")
            };

            var location = new Location
            {
                Coordinate = new Point(0, 0),
                Guild = guild,
                Name = "Разваливающаяся платина",
                Desc = "Старая развалюха в которую страшно зайти. Не удивительно что только самые отчаяные оборванцы осмеливаются искать тут дом",
                //HeroesInLocation = GetHeroes()
            };
            guild.Location = location;

            return guild;
        }

        public static List<Hero> GetHeroes(List<Skill> skills, 
            List<CharacteristicType> characteristicTypes, 
            List<StateType> stateTypes, 
            List<ThingSample> thingSamples)
        {
            var swordSample = thingSamples.First(x => x.Name == Sword);
            var armorBraSample = thingSamples.First(x => x.Name == ArmorBra);
            var healingPotionSample = thingSamples.First(x => x.Name == HealingPotion);
            var goldSample = thingSamples.First(x => x.Name == Gold);

            var baseSkills = skills.Where(x => x.School.Name == SchoolBaseSkillName).ToList();

            var freeman = new Hero
            {
                Name = "Freeman",
                Race = Race.Человек,
                Sex = Sex.Муж,
                Characteristics = GenerateStat(characteristicTypes, 1),
                Background = "Сын физика ядерщика",
                Skills = new List<Skill>()
            };
            freeman.Skills.AddRange(baseSkills);
            freeman.Skills.Add(skills[0]);
            freeman.Skills.Add(skills[1]);
            freeman.SetDefaultState(stateTypes);
            freeman.AddThingToHero(swordSample, true);
            freeman.AddThingToHero(goldSample, 100);
            freeman.AddThingToHero(healingPotionSample, 2);

            var shani = new Hero
            {
                Name = "Шани",
                Race = Race.Эльф,
                Sex = Sex.Жен,
                Characteristics = GenerateStat(characteristicTypes, 2),
                Background = "Дочь проститутки",
                Skills = new List<Skill>()
            };
            shani.Skills.AddRange(baseSkills);
            shani.Skills.Add(skills[3]);
            shani.Skills.Add(skills[4]);
            shani.SetDefaultState(stateTypes);
            shani.AddThingToHero(armorBraSample, true);
            shani.AddThingToHero(goldSample, 300);
            shani.AddThingToHero(swordSample, true);
            
            var ogrimar = new Hero
            {
                Name = "Огримар",
                Race = Race.Орк,
                Sex = Sex.Скрывает,
                Characteristics = GenerateStat(characteristicTypes, 3),
                Background = "В свои 14 трижды убивал",
                Skills = new List<Skill>()
            };
            ogrimar.Skills.AddRange(baseSkills);
            ogrimar.SetDefaultState(stateTypes);
            ogrimar.AddThingToHero(swordSample, true);
            ogrimar.AddThingToHero(goldSample, 10);
            ogrimar.AddThingToHero(healingPotionSample, 5);            

            var result = new List<Hero>
            {
                freeman,
                shani,
                ogrimar
            };

            return result;
        }

        public static List<ThingSample> GenerateThingSample(List<StateType> stateTypes)
        {
            var hp = stateTypes.First(x => x.Name == Hp);
            var dodge = stateTypes.First(x => x.Name == Dodge);
            var damage = stateTypes.First(x => x.Name == Damage);
            var armor = stateTypes.First(x => x.Name == Armor);

            var thingSamples = new List<ThingSample>();

            thingSamples.Add(new ThingSample
            {
                Name = Gold,
                Desc = "Золото, оно и в африке золото",
            });

            thingSamples.Add(new ThingSample
            {
                Name = Sword,
                Desc = "Вжух и готово",
                PassiveStates = new List<State> {
                    new State {
                        Number = 10,
                        StateType = damage
                    }
                }
            });

            thingSamples.Add(new ThingSample
            {
                Name = ArmorBra,
                Desc = "Вот хоть убей не найти эльфийку без этого важнейшего элемента одежды",
                RequrmentRace = Race.Эльф,
                RequrmentSex = Sex.Жен,
                PassiveStates = new List<State> {
                    new State {
                        Number = 5,
                        StateType = armor
                    }
                }
            });

            thingSamples.Add(new ThingSample
            {
                Name = HealingPotion,
                Desc = "Выпил и дыры затягиваются сами собой. Только щекотно",
                IsUsed = true,
                UsingEffectState = new List<State> {
                    new State {
                        Number = 10,
                        StateType = hp
                    }
                }
            });

            return thingSamples;
        }
        
        public static List<Characteristic> GenerateStat(List<CharacteristicType> characteristicTypes, int seed = 0)
        {
            var rnd = new Random(DateTime.Now.Millisecond + seed);
            var result = characteristicTypes.Select(characteristicType =>
                new Characteristic { CharacteristicType = characteristicType, Number = rnd.Next(1, 10) })
                .ToList();
            return result;
        }

        public static Quest QuestRat()
        {
            var quest = new Quest
            {
                Name = "Убить крыс",
                Desc = "Владелец амбара разметил заказ на убийство крыс. Отлично задание для новичка",
                Effective = 0,
            };

            quest.AllEvents = GenerateEventsForQuestRat(quest);

            return quest;
        }

        public static Quest QuestTower(List<CharacteristicType> characteristicTypes, List<StateType> stateTypes)
        {
            var quest = new Quest
            {
                Name = "Башня",
                Desc = "<p>	В великой Башне три уровня. Зачисти их все за один заход и получишь великий Кубок&nbsp;<span style=\"background-color:#ffff00;\">(добавить возможность награды для квеста)</span></p><p>	Перед тем как отправляться убедись что готов к сражениям</p><ol>	<li>		* Ловушки которые ранят если нет умения Уворот</li>	<li>		** Клады дают деньги</li>	<li>		*** Возможность подкупа за золото или обольстить если Пол и Красота на уровне</li>	<li>		**** Открыть короткий проход при помощи Силы или Ловкости</li></ol>",
                Effective = 0,
            };

            quest.AllEvents = GenerateEventsForQuestTower(quest, characteristicTypes, stateTypes);

            return quest;
        }

        public static List<Event> GenerateEventsForQuestRat(Quest quest)
        {
            // Tips
            // Desc = "У заказчика всегда была репутацию падкого на женское внимание мужика",
            // Desc = "В общем орков не любят со времён третьей общей войны, но порой можно встретить общины с прямо противоположным мнением",

            var lvl0Event0 = new Event
            {
                Name = "Начало",
                //ParentEvents = null,
                ProgressChanging = 30,
                Desc =
                    "Герой по не опытности целый день плутал по городу в поисках торговца, который сделал заказ на убийство крыс",
            };
            var lvl1Event1 = new Event
            {
                Name = "Баба заигрывает",
                ProgressChanging = 50,
                RequrmentSex = Sex.Жен,
                Desc =
                    "Героиня легко и непренуждённо пообщалась с заказчиком, после чего смогла убедить его снизить требования к выполнению задания",
            };
            var lvl1Event2 = new Event
            {
                Name = "Мужика послали",
                ProgressChanging = -30,
                RequrmentSex = Sex.Муж,
                Desc =
                    "Герой не успел и представиться как заказчик с недовольством начал указывать на недопустимость подобного поведения. Пришлось узнавать детали о задание у простых служащих, на что ушло в два раза больше сил и времени"
            };
            var lvl1Event3 = new Event
            {
                Name = "Скрытен работает",
                ProgressChanging = 10,
                RequrmentSex = Sex.Скрывает,
                Desc =
                    "Заказчик долго рассматривал героя, то недовольно бурча под нос, но натягивая приветливую улыбку. Тем временем вся необходимая информация была полученна и герой отправился дальше"
            };

            var lvl1Event4 = new Event
            {
                Name = "Перевёл дыхание",
                ProgressChanging = 10,
                Desc = "Получив все необходимые детали, герой уже было решил отправиться на прямую к амбару в котором были замечены крысы, но вспомнил наставления учителя. \"Что мы говорим основному квесту? Не сегодня\" и решил ещё день побродить по окрестностям в поисках приключений для своей мягкой точки",
            };

            var lvl2Event1 = new Event
            {
                Name = "Эльфу сложней",
                ProgressChanging = -30,
                RequrmentRace = Race.Эльф,
                Desc =
                    "Герою постоянно строили козни местные жители. Пришлось потратить много времени и сил на убеждение в своей добропорядочности",
            };
            var lvl2Event2 = new Event
            {
                Name = "Орку легче",
                ProgressChanging = 50,
                RequrmentRace = Race.Орк,
                Desc =
                    "Все вокруг пытались помочь Герою. Ничего полезного местные рассказать не смогли, но вот снаряжения надавали отменного, что явно помогло"
            };
            var lvl2Event3 = new Event
            {
                Name = "Человеку пофиг",
                ProgressChanging = 0,
                Desc = "Пользы от бесцельно проведённого дня было мало, зато герой успокаивал себя мыслью, что точно не пропустил возможных приключений. Возможно в следующий раз ему повезёт больше"
            };

            var lvl3Event0 = new Event
            {
                Name = "Конец",
                ProgressChanging = 30,
                Desc = "Герой нашёл злосчастных крыс и безжалостно уничтожил всех кого смог догнать",
            };

            var lvl0Event0LinksFrom = lvl0Event0.LinksFromThisEvent ?? new List<EventLinkItem>();
            lvl0Event0.AddChildEvent(lvl1Event1);
            lvl0Event0.AddChildEvent(lvl1Event2);
            lvl0Event0.AddChildEvent(lvl1Event3);

            lvl1Event4.AddParentEvent(lvl1Event1);
            lvl1Event4.AddParentEvent(lvl1Event2);
            lvl1Event4.AddParentEvent(lvl1Event3);

            lvl1Event4.AddChildEvent(lvl2Event1);
            lvl1Event4.AddChildEvent(lvl2Event2);
            lvl1Event4.AddChildEvent(lvl2Event3);

            lvl3Event0.AddParentEvent(lvl2Event1);
            lvl3Event0.AddParentEvent(lvl2Event2);
            lvl3Event0.AddParentEvent(lvl2Event3);

            var list = new List<Event>();
            list.Add(lvl0Event0);

            list.Add(lvl1Event1);
            list.Add(lvl1Event2);
            list.Add(lvl1Event3);
            list.Add(lvl1Event4);

            list.Add(lvl2Event1);
            list.Add(lvl2Event2);
            list.Add(lvl2Event3);

            list.Add(lvl3Event0);

            list.ForEach(x => x.Quest = quest);

            quest.RootEvent = lvl0Event0;

            return list;
        }

        public static List<Event> GenerateEventsForQuestTower(Quest quest, List<CharacteristicType> characteristicTypes, List<StateType> stateTypes)
        {
            // Tips
            // Desc = "У заказчика всегда была репутацию падкого на женское внимание мужика",
            // Desc = "В общем орков не любят со времён третьей общей войны, но порой можно встретить общины с прямо противоположным мнением",
            var agilityType = characteristicTypes.First(x => x.Name == Agility);
            var hpType = stateTypes.First(x => x.Name == Hp);

            var event0 = new Event
            {
                Name = "Евент 00. Вход",
                Desc = "Подойдя к Башне Герой осмотрел предстоящий путь. Облака скрывали вершину рукотворного чуда. Видимо информация о трёх этажах несколько преумешина. Но до входной двери ещё предстоило добраться. Башню со все сторон окружает ров заполненный водой. Через ров перекинут мост, но по большей части он разрушен. Звонкие всплески намекают, что в воде есть живность, но вот определить размеры и агресивность пока непредставляется возможным. Теоретически можно воспользоваться мостом, но несколько раз придётся перепрыгивать обрушеные части моста да и в целом данная затея предвещает множество акробатических приёмов ошибка в которых будет стоить серьёзных ушибов. С другой стороны плаванье было обязательным предметом в школе героев так что переплыть ров в наиболее узком месте более чем надёжный способ оказатся с по ту сторону проблем.",
            };
            var event1 = new Event
            {
                Name = "Евент 01. Мост",
                Desc = "Не долго думаю Герой набрал скорость и совершил рывок вверх по мосту. Позади оказался первый провал. Несколько шагов и ещё прыжок. Казалось всё идёт по плану, но случилось невероятное, на поверхности моста оказалась лужа! Правая нога уходит в сторону, а тело продолжая подчинятся энерции движется впередё.",
            };
            var event2 = new Event
            {
                Name = "Евент 02. Мост. Удача",
                Desc = "Сальто, потом кувырок и невероятноя гордость за себя. Вот так незамысловато разрешилась опасная ситуация на мосту. Повысив свою самооценку герой уверен направился к входу в башню",
                RequrmentCharacteristics = new List<Characteristic> {
                    new Characteristic { Number = 6, CharacteristicType = agilityType }
                }
            };
            var event3 = new Event
            {
                Name = "Евент 03. Мост. Провал",
                Desc = "Камень оказался холодным, твердым и совершенно безразличным к страданиям героя. Он даже не соизволил пошевелиться когда матерящееся лицо врезалось в него. Благо в Герои кого попало не берут. Потому не теряя достоинство, Герой несколько раз отдуши пнул виновника кровавого подтёка на щеке и удалился к входу в башню",
                HeroStatesChanging = new List<State> {
                    new State { Number = -5, StateType = hpType }
                },
            };
            var event4 = new Event
            {
                Name = "Евент 04. Река",
                Desc = "Осмотрев другой берег герой нашёл подходящее место. Сняв всё то что может промокнуть Герой смело направился в воду. Вот кто бы сомневался что на первый укус рыбы отважатся лишь посреди пути, когда пути назад не будет. Тихо ненавидя всех водоплавающих Герой добрался до другого берега с несколько меньшим количество крови в его теле чем было до входа в реку.",
                HeroStatesChanging = new List<State> {
                    new State { Number = -3, StateType = hpType }
                },
            };
            var event5 = new Event
            {
                Name = "Евент 05. Вход в Башню",
                Desc = "Герой преодолел водную преграду и смело вошёл в Башню. Какого же было его удивление когда сразу за дверью его ожидала улыбающаяся орчиха.",
            };
            var event6 = new Event
            {
                Name = "Евент 06. Первый этаж. Мирный путь",
                Desc = "Ответив симетричной улыбкой Герой двигаясь хоть и несколько настороженно, но всё же достаточно свободно.",
                RequrmentRace = Race.Орк,
            };
            var event7 = new Event
            {
                Name = "Евент 07. Первый этаж. Сражение",
                Desc = "Быстрый удар Героя пришёлся по месту. Но вот о чём забыл Герой, так это то что его противник Орк. Глубокая рана конечно мешала орчихе сражаться, но всё же несколько раз она смогла задеть Героя",
                HeroStatesChanging = new List<State> {
                    new State { Number = -2, StateType = hpType }
                },
            };
            var event8 = new Event
            {
                Name = "Евент 08. Второй этаж",
                Desc = "Пожалуй, на сегодня хватит, пойду-ка я домой, а не на ваш второй этаж",
            };

            event0.AddChildEvent(event1, "Рискованные прыжки мой хлеб! Иду на мост");
            event0.AddChildEvent(event4, "В таком пруду больших хищных рыб точно не будет! Так что лучше я вплавь отправлюсь");
            event1.AddChildEvent(event2, "(Л-6) А сальто разве не специально для таких случаев придуманно? Ловко сделать кувырок в воздухе, подправив свою траекторию полёта руками по ходу дела.");
            event1.AddChildEvent(event3, "Ой что это? Булыжник с огромной скоростью приближается к моей голове! Хотя нет, всё впорядке, камень находится в покое, это просто моя голова стремится в его сторону.");
            event5.AddParentsEvents("К Башне", event2, event3, event4);
            event5.AddChildEvent(event6, "(Орк)Мы орки отлично знаем, если правый клык при улыбке выше левого, то это дружелюбная улыбка");
            event5.AddChildEvent(event7, "Говорят звериному оскалу можно противопоставить лишь звериный оскал. Это конечно же ложь. Внезапная смерельная атака отлично справляется с задаче умиротворения любого оскала");
            event8.AddParentsEvents("Идём к лестнице на второй этаж", event6, event7);

            var list = new List<Event>();

            list.AddRange(new List<Event> {
                event0,
                event1,
                event2,
                event3,
                event4,
                event5,
                event6,
                event7,
                event8,
            });

            list.ForEach(x => x.Quest = quest);

            quest.RootEvent = event0;

            return list;
        }

        public static List<Skill> GenerateSkills(List<SkillSchool> schools, List<StateType> stateTypes)
        {
            var baseSchool = schools.FirstOrDefault(x => x.Name == "Базовые умения");
            var coldSchool = schools.FirstOrDefault(x => x.Name == "Школа льда");
            var fireSchool = schools.FirstOrDefault(x => x.Name == "Школа пламени");
            var niceSchool = schools.FirstOrDefault(x => x.Name == "Соблазнения");

            var hp = stateTypes.First(x => x.Name == Hp);
            var mp = stateTypes.First(x => x.Name == Mp);

            var skills = new List<Skill>();

            /* ************ Fire ************ */
            skills.Add(new Skill
            {
                Name = "Fire ball",
                Desc = "Fire ball",
                School = fireSchool,
                SelfChanging = new List<State> { new State { StateType = mp, Number = -4 } },
                TargetChanging = new List<State> { new State { StateType = hp, Number = -6 } }
            });

            skills.Add(new Skill
            {
                Name = "Pyro blast",
                Desc = "Pyro blast",
                School = fireSchool,
                SelfChanging = new List<State> { new State { StateType = mp, Number = -10 } },
                TargetChanging = new List<State> { new State { StateType = hp, Number = -10 } },
            });

            /* ************ Cold ************ */
            skills.Add(new Skill
            {
                Name = "Ice spear",
                Desc = "Ice spear",
                School = coldSchool,
                SelfChanging = new List<State> { new State { StateType = mp, Number = -2 } },
                TargetChanging = new List<State> { new State { StateType = hp, Number = -3 } },
            });

            skills.Add(new Skill
            {
                Name = "Ice armor",
                Desc = "Ice armor",
                School = coldSchool,
                SelfChanging = new List<State>
                {
                    new State { StateType = mp, Number = -4 },
                    new State { StateType = hp, Number = 10 }
                }
            });

            /* ************ Seduction ************ */
            skills.Add(new Skill
            {
                Name = "69",
                Desc = "No question, please. It's working and that all what you need to know",
                School = niceSchool,
            });

            /* ************ Base ************ */
            skills.Add(new Skill
            {
                Name = "Удар рукой",
                Desc = "Что может быть проще?",
                School = baseSchool,
                TargetChanging = new List<State> { new State { StateType = hp, Number = -2 } },
            });

            skills.Add(new Skill
            {
                Name = "Блок щитом",
                Desc = "Кто хочет жить, использует щит",
                School = baseSchool,
                SelfChanging = new List<State> { new State { StateType = hp, Number = 2 } },
            });

            skills.Add(new Skill
            {
                Name = "Уворот",
                Desc = "Для тех кто хочет умереть красиво",
                School = baseSchool,
                SelfChanging = new List<State> { new State { StateType = hp, Number = 1 } },
            });

            return skills;
        }

        public static List<TrainingRoom> GenerateTrainingRooms(List<SkillSchool> schools)
        {
            var baseSchool = schools.FirstOrDefault(x => x.Name == "Базовые умения");
            var coldSchool = schools.FirstOrDefault(x => x.Name == "Школа льда");
            var fireSchool = schools.FirstOrDefault(x => x.Name == "Школа пламени");
            var niceSchool = schools.FirstOrDefault(x => x.Name == "Соблазнения");

            var rooms = new List<TrainingRoom>();

            rooms.Add(new TrainingRoom
            {
                Name = "Жаровня",
                School = fireSchool,
                Price = 300
            });

            rooms.Add(new TrainingRoom
            {
                Name = "Холодильник",
                School = coldSchool,
                Price = 120
            });

            rooms.Add(new TrainingRoom
            {
                Name = "Бар",
                School = niceSchool,
                Price = 500
            });

            rooms.Add(new TrainingRoom
            {
                Name = "Комната посвящения",
                School = baseSchool,
                Price = 10
            });

            return rooms;
        }

        public static List<StateType> GenerateStateTypes()
        {
            var stateTypes = new List<StateType>();

            stateTypes.Add(new StateType
            {
                Name = MaxHp,
                Desc = "Сколько куриц не ешь, больше жизней в тебя просто не вместится",
            });

            stateTypes.Add(new StateType
            {
                Name = MaxMp,
                Desc = "Сколько зелей не пей, больше маны в тебя просто не вместится",
            });

            stateTypes.Add(new StateType
            {
                Name = Hp,
                Desc = "Опустить до нуля и ты труп",
            });

            stateTypes.Add(new StateType
            {
                Name = Mp,
                Desc = "Нет маны, нет заклинаний",
            });

            stateTypes.Add(new StateType
            {
                Name = Gold,
                Desc = "Что может быть лучше кошелька с золотыми? Мешок с золотыми!",
            });

            stateTypes.Add(new StateType
            {
                Name = Dodge,
                Desc = "Борис хрен попадёшь, на всегда останеться недостижимым идеалом",
            });

            stateTypes.Add(new StateType
            {
                Name = Damage,
                Desc = "Урон за удар. Интересно какой параметр у ВанПанчмента?",
            });

            stateTypes.Add(new StateType
            {
                Name = Armor,
                Desc = "Вот попробуй пробить закованного в полный доспех парня",
            });

            return stateTypes;
        }

        public static List<CharacteristicType> GenerateCharacteristicType(List<StateType> stateTypes)
        {
            var maxHp = stateTypes.First(x => x.Name == MaxHp);
            var hp = stateTypes.First(x => x.Name == Hp);
            var dodge = stateTypes.First(x => x.Name == Dodge);
            //var gold = stateTypes.First(x => x.Name == Gold);
            var damage = stateTypes.First(x => x.Name == Damage);
            var armor = stateTypes.First(x => x.Name == Armor);
            var characteristicType = new List<CharacteristicType>();

            characteristicType.Add(new CharacteristicType
            {
                Name = Strength,
                Desc = "Чем сильней, тем тупей, шутят над орками. А те почему-то смеются и бьют себя в голову",
                EffectState = new List<State>
                {
                    new State { StateType = maxHp, Number = 5 },
                    new State { StateType = hp, Number = 5 },
                    new State { StateType = damage, Number = 2 },
                    new State { StateType = armor, Number = 1 }
                }
            });

            characteristicType.Add(new CharacteristicType
            {
                Name = Agility,
                Desc = "Удержать яйцо на иголке? Легко!",
                EffectState = new List<State> {
                    new State { StateType = dodge, Number = 5 },
                    new State { StateType = damage, Number = 1 },
                }
            });

            characteristicType.Add(new CharacteristicType
            {
                Name = Charism,
                Desc = "Это не честно кричала некрасивая девочка, глядя как выходит за муж очередная подруга",
                EffectState = new List<State> {
                    //new State { StateType = gold, Number = 10 }
                    new State { StateType = dodge, Number = 2 }
                }
            });

            return characteristicType;
        }

        public static List<SkillSchool> GenerateSchools()
        {
            var schools = new List<SkillSchool>();

            schools.Add(new SkillSchool
            {
                Name = SchoolBaseSkillName,
                Desc = "Доступны всем по умолчанию. Ну разве что кроме совсем 'одарённых'"
            });

            schools.Add(new SkillSchool
            {
                Name = SchoolColdSkillName,
                Desc = "Охладись и начинай"
            });

            schools.Add(new SkillSchool
            {
                Name = SchoolFireSkillName,
                Desc = "Для тех кто любит зажигать"
            });

            schools.Add(new SkillSchool
            {
                Name = SchoolNiceSkillName,
                Desc = "Всё то что поможет вам убедить людей, без грубой силой"
            });

            return schools;
        }

        public static Hero GetDefaultHero(List<StateType> stateTypes, List<CharacteristicType> characteristicTypes, List<Skill> skills)
        {
            var hero = new Hero();

            hero.State = stateTypes.Select(x => new State { Number = 1, StateType = x }).ToList();
            hero.Characteristics = characteristicTypes.Select(x => new Characteristic { Number = 1, CharacteristicType = x }).ToList();
            hero.Skills = skills;

            return hero;
        }
    }
}
