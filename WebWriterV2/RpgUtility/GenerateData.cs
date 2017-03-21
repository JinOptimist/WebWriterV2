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

        public const string FireBall = "Fire ball";
        public const string EvasionSkill = "Уворот";


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
                Desc = "Что может быть лучше кошелька с золотыми? Мешок с золотыми!",
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

        public static Quest QuestRat()
        {
            var quest = new Quest
            {
                Name = "Убить крыс",
                Desc = "Владелец амбара разметил заказ на убийство крыс. Отлично задание для новичка",
            };

            quest.AllEvents = GenerateEventsForQuestRat(quest);

            return quest;
        }

        public static Quest QuestTower(List<StateType> stateTypes,
            List<ThingSample> thingSamples)
        {
            var quest = new Quest
            {
                Name = "Башня",
                Desc = "<p>	В великой Башне три уровня. Зачисти их все за один заход и получишь великий Кубок&nbsp;<span style=\"background-color:#ffff00;\">(добавить возможность награды для квеста)</span></p><p>	Перед тем как отправляться убедись что готов к сражениям</p><ol>	<li>		* Ловушки которые ранят если нет умения Уворот</li>	<li>		** Клады дают деньги</li>	<li>		*** Возможность подкупа за золото или обольстить если Пол и Красота на уровне</li>	<li>		**** Открыть короткий проход при помощи Силы или Ловкости</li></ol>",
            };

            quest.AllEvents = GenerateEventsForQuestTower(quest, stateTypes, thingSamples);

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
                Desc =
                    "Героиня легко и непренуждённо пообщалась с заказчиком, после чего смогла убедить его снизить требования к выполнению задания",
            };
            var lvl1Event2 = new Event
            {
                Name = "Мужика послали",
                ProgressChanging = -30,
                Desc =
                    "Герой не успел и представиться как заказчик с недовольством начал указывать на недопустимость подобного поведения. Пришлось узнавать детали о задание у простых служащих, на что ушло в два раза больше сил и времени"
            };
            var lvl1Event3 = new Event
            {
                Name = "Скрытен работает",
                ProgressChanging = 10,
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
                Desc =
                    "Герою постоянно строили козни местные жители. Пришлось потратить много времени и сил на убеждение в своей добропорядочности",
            };
            var lvl2Event2 = new Event
            {
                Name = "Орку легче",
                ProgressChanging = 50,
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

        public static List<Event> GenerateEventsForQuestTower(Quest quest,
            List<StateType> stateTypes,
            List<ThingSample> thingSamples)
        {
            // Tips
            // Desc = "У заказчика всегда была репутацию падкого на женское внимание мужика",
            // Desc = "В общем орков не любят со времён третьей общей войны, но порой можно встретить общины с прямо противоположным мнением",
            var hpType = stateTypes.First(x => x.Name == Hp);

            var gold100 = new Thing
            {
                Count = 100,
                ThingSample = thingSamples.First(x => x.Name == Gold)
            };

            var gold300 = new Thing
            {
                Count = 300,
                ThingSample = thingSamples.First(x => x.Name == Gold)
            };

            var healingPotion = new Thing
            {
                Count = 1,
                ThingSample = thingSamples.First(x => x.Name == HealingPotion)
            };


            var event0 = new Event
            {
                Name = "Евент 00. Вход",
                Desc = "Подойдя к Башне Герой осмотрел предстоящий путь. Облака скрывали вершину рукотворного чуда. Видимо информация о трёх этажах несколько преуменьшена. Но до входной двери ещёпредстояло добраться. Башню со все сторон окружает ров заполненный водой. Через ров перекинут мост, но по большей части он разрушен. Звонкие всплески намекают, что в воде есть живность, но вот определить размеры и агрессивность пока не представляется возможным. Теоретически можно воспользоваться мостом, но несколько раз придётся перепрыгивать обрушенные части моста да и в целом данная затея предвещает множество акробатических приёмов ошибка в которых будет стоить серьёзных ушибов. С другой стороны плаванье было обязательным предметом в школе героев так что переплыть ров в наиболее узком месте более чем надёжный способ оказаться с по ту сторону проблем.",
            };
            var event1p0 = new Event
            {
                Name = "Евент 01. Мост",
                Desc = "У подножья поста Герой с удивлением обнаружил лечебное зелье. Мысленное поблагодарив недотёпу потерявшего этот простой но весьма полезный предмет герой осмотрел предстоящий путь. Однако поймав себя на мысли, что настоящий герой не должен колебаться, Герой не долго думая набрал скорость и совершил рывок вверх по мосту. Позади оказался первый провал. Несколько шагов и ещё прыжок. Казалось всё идёт по плану, но случилось невероятное, на поверхности моста оказалась лужа! Правая нога уходит в сторону, а тело продолжая подчинятся инерции движется вперёд.",
                ThingsChanges = new List<Thing> { healingPotion }
            };
            var event1p1 = new Event
            {
                Name = "Евент 01. Мост. Удача",
                Desc = "Сальто, потом кувырок и невероятная гордость за себя. Вот так незамысловато разрешилась опасная ситуация на мосту. Повысив свою самооценку герой уверен направился к входу в башню",
            };
            var event1p2 = new Event
            {
                Name = "Евент 01. Мост. Провал",
                Desc = "Камень оказался холодным, твердым и совершенно безразличным к страданиям героя. Он даже не соизволил пошевелиться когда матерящееся лицо врезалось в него. Благо в Герои кого попало не берут. Потому не теряя достоинство, Герой несколько раз от души пнул виновника кровавого подтёка на щеке и удалился к входу в башню",
                HeroStatesChanging = new List<State> {
                    new State { Number = -5, StateType = hpType }
                },
            };
            var event2 = new Event
            {
                Name = "Евент 02. Река",
                Desc = "Осмотрев другой берег герой нашёл подходящее место. Сняв всё то что может промокнуть Герой смело направился в воду. Вот кто бы сомневался что на первый укус рыбы отважатся лишь посреди пути, когда пути назад не будет. Тихо ненавидя всех водоплавающих Герой добрался до другого берега с несколько меньшим количество крови в его теле чем было до входа в реку.",
                HeroStatesChanging = new List<State> {
                    new State { Number = -3, StateType = hpType }
                },
            };
            var event3 = new Event
            {
                Name = "Евент 03. Вход в Башню",
                Desc = "Герой преодолел водную преграду и смело вошёл в Башню. Какого же было его удивление когда сразу за дверью его ожидала улыбающаяся орчиха.",
            };
            var event6p1 = new Event
            {
                Name = "Евент 06. Первый этаж. Мирный путь",
                Desc = "Ответив симметричной улыбкой Герой поздоровался и сообщил, что ему предстоят великие свершения. Несколько рыков в подтверждения серьёзности своих намерений и диалог плавно перешёл от напряжённой притирки друг к другу к задорному хвастовству у кого больше шрамов.",
            };
            var event6p2 = new Event
            {
                Name = "Евент 06. Первый этаж. Огонь",
                Desc = "Быстрому удару Герой предпочёл внезапный огненный шар сжигающий врага. И тут ужу не играло роли кто окажется на пути безжалостного пламени. Победа была мгновенной и бескровной, лишь шипящий орк в углу.",
            };
            var event6p3 = new Event
            {
                Name = "Евент 06. Первый этаж. Сражение",
                Desc = "Быстрый удар Героя пришёлся по месту. Но вот о чём забыл Герой, так это то что его противник Орк. Глубокая рана конечно мешала орчихе сражаться, но всё же несколько раз она смогла задеть Героя. В итоге Герой решил не испытывать судьбу и оставив раненую орчиху позади двинулся дальше. Благо она так же не горела желанием продолжать бой в явно невыгодной ситуации.",
                HeroStatesChanging = new List<State> {
                    new State { Number = -2, StateType = hpType }
                },
            };
            var event8 = new Event
            {
                Name = "Евент 08. Второй этаж",
                Desc = "Как и следовало ожидать винтовая лестница использовалась в том числе как склад. Порыскав под лестницей герой обнаружил мешок с золотом. Нет, не такой огромный что теперь можно уйти на покой и больше не заниматься опасным ремеслом, но новый меч или даже доспех приобрести на эти средства вполне было возможно. И это обстоятельство заставило Героя задуматься, возможно ну его, этот ваш второй этаж поход уже явно окупился.",
                ThingsChanges = new List<Thing> { gold100 }
            };
            var event8p1 = new Event
            {
                Name = "Евент 08. Второй этаж. Ловушка",
                Desc = "Какой интересный «щёлк», подумал герой смело шагая по второму этажу безлюдной башни. И был абсолютно прав. Жаль только мало кто из посетителей башни оценил по достоинству сложность механизмов нажимной плиты, приводящей в действие арбалеты заботливо расставленные в соседней комнате. А ведь один ответственный гоблин изо дня в день проверял чистил, заряжал эту и многие другие ловушки. Думаете ему хоть раз сказали «Молодец, отлично поработал»? Конечно нет. Никто не ценил его труд.",
            };

            var event8p2 = new Event
            {
                Name = "Евент 08. Второй этаж. Ловушка. Провал",
                Desc = "Любопытный, но не слишком сообразительный герой ещё доли секунды витал в облаках, пока по башне не разнёсся крик полный боли и злобы. Проклиная всех кто участвовал в создание ловушки начиная с инженера спроектировавшего данный механизм и заканчивая гномом добывшим руду из которой выплавили наконечник для стрелы торчащей из колена Герой проковылял опасный участок и занялся обработкой раны.",
                HeroStatesChanging = new List<State> {
                    new State { Number = -5, StateType = hpType }
                },
            };
            var event8p3 = new Event
            {
                Name = "Евент 08. Второй этаж. Ловушка. Удача",
                Desc = "Лениво выписывая пируэты и продолжая движение вперёд, Герой продолжил отрёчённые размышления о том как сложно в современном мире найти достойное приключение для профессионального героя. Вот другое дело в старые добрые времена, когда геройствовать ходили необученные недоросли способные получить стрелой в колено на таких вот старых как мир ловушках.",
            };
            var event8p4 = new Event
            {
                Name = "Евент 08. Второй этаж. Награда",
                Desc = "Уже нисколько не удивляясь наличию очередного мешка с золотыми у выхода со второго этажа Герой оценил на вес награду за прохождение второго этажа. Она казалась раза в три весомее чем предыдущая. Рассудив что вот теперь-то точно настал тот момент когда пора сворачивать удочку, герой повернул назад",
                ThingsChanges = new List<Thing> { gold300 }
            };
            var event99 = new Event
            {
                Name = "Евент 99. Выход",
                Desc = "Герой к большому удивлению смог выбраться из башни без повторного прохождения всех этих ловушек и сложных выборов в стиле разбить голову об камень или быть искусанным хищными рыбами. Стоило ему повернуть обратно с твёрдым решением вернутся домой живым, как сработало телепортирующее заклинание и герой оказался в гильдии. Конец.",
            };

            event0.AddChildEvent(event1p0, "Рискованные прыжки мой хлеб! Иду на мост");
            event0.AddChildEvent(event2, "В таком пруду больших хищных рыб точно не будет! Так что лучше я вплавь отправлюсь.");
            event1p0.AddChildEvent(event1p1, "(Л-6) А сальто разве не специально для таких случаев придумано? Ловко сделать кувырок в воздухе, подправив свою траекторию полёта руками по ходу дела.");
            event1p0.AddChildEvent(event1p2, "Ой что это? Булыжник с огромной скоростью приближается к моей голове! Хотя нет, всё в порядке, камень находится в покое, это просто моя голова стремится в его сторону.");
            event3.AddParentsEvents("К Башне", event1p1, event1p2, event2);
            event3.AddChildEvent(event6p1, "(Орк)Мы орки отлично знаем, если правый клык при улыбке выше левого, то это дружелюбная улыбка.");
            event3.AddChildEvent(event6p2, "(Огненный шар) Добро... зло... Главное у кого огненный шар в рукаве. Сжечь это недоразумение посмевшее стать на пути у огненного мага!");
            event3.AddChildEvent(event6p3, "Говорят звериному оскалу можно противопоставить лишь звериный оскал. Это конечно же ложь. Внезапная смертельная атака отлично справляется с задаче умиротворения любого оскала.");

            event8.AddParentsEvents("Идём к лестнице на второй этаж", event6p1, event6p2, event6p3);
            event8.AddChildEvent(event99, "Осторожный, синоним к слову Живой. Так что лучше отправится восвояси");
            event8.AddChildEvent(event8p1, "Издеваетесь? Я только разогрелся! Вбежать на второй этаж.");
            event8p1.AddChildEvent(event8p2, "Был щёлк, а теперь вот ещё какой-то Дзынь, интересно что бы это значило");
            event8p1.AddChildEvent(event8p3, "(Уворот) Конечно не ценили! Пружины взведены лишь на треть. Стрелы летят с такой скоростью, что только сонный гном, мог бы не заметить их. Всего один перекат в сторону и стрелы");
            event8p4.AddParentsEvents("К выходу с этажа", event8p2, event8p3);
            event8p4.AddChildEvent(event99, "Exit");

            var list = new List<Event>();

            list.AddRange(new List<Event> {
                event0,
                event1p0,
                event1p1,
                event1p2,
                event2,
                event3,
                event6p1,
                event6p2,
                event6p3,
                event8,
                event8p1,
                event8p2,
                event8p3,
                event8p4,
                event99,
            });

            list.ForEach(x => x.Quest = quest);

            quest.RootEvent = event0;

            return list;
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

        public static Hero GetDefaultHero(List<StateType> stateTypes)
        {
            var hero = new Hero();

            //hero.State = stateTypes.Select(x => new State { Number = 1, StateType = x }).ToList();
            //hero.Characteristics = characteristicTypes.Select(x => new Characteristic { Number = 1, CharacteristicType = x }).ToList();

            hero.State = new List<State>();

            return hero;
        }
    }
}
