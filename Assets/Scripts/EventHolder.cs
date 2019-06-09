using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EventHolder{

    private GameController gameController;
    public List<Event> events = new List<Event>();
    int test = 0;

    delegate void testD();

    public EventHolder()
    {

    }

    public void Init(GameController gameController)
    {
        this.gameController = gameController;
        Load();
    }

    public Event GetEventByID(string ID)
    {
        Event ev = events.Find(e => e.Id == ID);
        return ev;
    }


    void Load()
    {
        Event ev = new Event();
        //

        #region "normal_shop"
        events.Add(new Event()
        {
            //店の初期化自体はマップ生成時に行う
            Id = "normal_shop",
            Texts = new List<string>()
            {
                "冒険者たちは商人に出会った。"
            },
            OnEnd = (() =>
            {
                gameController.CurrentEvent = GetEventByID("normal_shop_option");
            }),
            //NextID = "normal_shop_option"
        });
        events.Add(new Event()
        {
            Id = "normal_shop_option",
            TopText = "商人「いらっしゃい。安くしておくよ」",
            OptionIDs = new List<(string, string)>()
            {
                ("アイテムを購入", "normal_shop_result01"),
                ("アイテムを売却", "normal_shop_result02"),
                ("強盗を行う(カルマ値+40)", "normal_shop_result03"),
                ("立ち去る", "normal_shop_result04")
            }
            //NextID = "hungry_human_result"
        });
        events.Add(new Event()
        {
            Id = "normal_shop_result01",
            Texts = new List<string>()
            {
            },
            OnEnd = (() =>
            {
                gameController.CurrentCell.Shop.OpenBuyingWindow();
                gameController.CurrentEvent = GetEventByID("normal_shop_option");
            }),
        });
        events.Add(new Event()
        {
            Id = "normal_shop_result02",
            Texts = new List<string>()
            {
            },
            OnEnd = (() =>
            {
                gameController.CurrentCell.Shop.OpenSellingWindow();
                gameController.CurrentEvent = GetEventByID("normal_shop_option");
            }),
        });
        events.Add(new Event()
        {
            Id = "normal_shop_result03",
            Texts = new List<string>()
            {
                "強盗を行った。"
            },
            OnEnd = (() =>
            {
                gameController.CurrentEvent = null;
            })
        });
        events.Add(new Event()
        {
            Id = "normal_shop_result04",
            Texts = new List<string>()
            {
                "さようなら。"
            },
            OnEnd = (() =>
            {
                gameController.CurrentEvent = null;
            })
        });
        #endregion

        #region "hungly_human"
        events.Add(new Event()
        {
            Id = "hungry_human",
            Texts = new List<string>()
            {
                "冒険者たちが歩みを進めていると、一人の男が声をかけてきた。",
                "「お願いします。少しでいいので物資を分けてくれませんか？」\n" +
                "見ると、男はかなり弱りきっている。\n" +
                "どうやら相当に困っている様子だ。",
                "冒険者たちは物資を分け与えてもいいし、分け与えなくてもいい。\n" +
                "あるいは、弱り切っている彼から物資を奪い去ることもできるが…。"
            },
            NextID = "hungry_human_option"
        });
        events.Add(new Event()
        {
            Id = "hungry_human_option",
            TopText = "どうしようか？",
            OptionIDs = new List<(string, string)>()
            {
                ("物資を分ける(<color=red>燃料-20</color>, <color=blue>カルマ値-20</color>)", "hungry_human_result01"),
                ("物資を分けない(カルマ値<color=red>+5</color>)", "hungry_human_result02"),
                ("物資を奪う(アイテム入手, カルマ値<color=red>+30</color>)", "hungry_human_result03")
            }
        //NextID = "hungry_human_result"
        });
        events.Add(new Event()
        {
            Id = "hungry_human_result01",
            Texts = new List<string>()
            {
                "「おお神よ。優しき冒険者たちに幸あらんことを…。」",
                "冒険者達は物資こそ失ったが、少し<color=orange>安らかな気持ち</color>になった。" +
                "<color=yellow>燃料-20、カルマ値-20</color>"
            },
            OnEnd = (() => 
            {
                gameController.SetFuel(-20);
                gameController.SetKarma(-20);
            })
        });
        events.Add(new Event()
        {
            Id = "hungry_human_result02",
            Texts = new List<string>()
            {
                "彼のことは気になるが、しかし我々とて余裕があるわけではない。" +
                "冒険者達は断りを入れて立ち去った。",
                "冒険者たちの心に、わずかな罪悪感が芽生えた。<color=red>カルマ値+5</color>"
            },
            OnEnd = (() =>
            {
                gameController.SetKarma(5);
            })
        });
        events.Add(new Event()
        {
            Id = "hungry_human_result03",
            Texts = new List<string>()
            {
                "冒険者たちの頭に、ある種<color=orange>悪魔的な発想</color>が浮かんだ。\n" +
                "なに、ここはダンジョン、準備を怠ったほうが悪いのだ。",
                "「悪く思うなよ。じゃあな」\n" +
                "冒険者たちは彼から強引に持ち物を奪い去ると、叫ぶ彼を後ろに、足早に立ち去った。",
                "冒険者たちの心に、<color=orange>罪悪感</color>が芽生えた。<color=red>カルマ値+15</color>"
            },
            OnEnd = (() =>
            {
                gameController.SetKarma(15);
            }),
            //ItemInfo = new Event.GotItemInfo
            //{
            //    Num = 1,
            //    Rarity = (0, 60)
            //}
        });
        #endregion
        #region "holy_knights"
        events.Add(new Event()
        {
            Id = "holy_knights",
            Texts = new List<string>()
            {
                "冒険者たちの前に、質の高そうな装備で身を固めた騎士の集団が現れた。\n" +
                "「止まれ！」",
                "「我々は<color=orange>帝国の聖騎士団</color>だ。この地は一応だが、領土的には我が国の管轄下にある」",
                "「我々とて、こんな辺境の怪しい土地にまで足を運びたくはないが…貴様らのような\n" +
                "馬鹿な連中が面倒を起こしがちだから、こうして見回りに来ているわけだ」\n" +
                "彼らの代表はそう言いながら、我々にほとんど興味がないかのようによそを向いている。",
                "「さて…本題に入ろうか。」彼は我々の方に向き直って言った。\n" +
                "「実は貴様らが、この地でたびたび人道に反する行為を行っているとの報告があってな。" +
                "こちらとしては、<color=orange>然るべき対処</color>を取らざるを得ん」",
                "「早い話が、国の法律に則って<color=orange>罰金</color>を払ってもらわねばならん。" +
                "今回の件は、それで水に流してやる。どうだ？」"
            },
            NextID = "holy_knights_options",
            OnEnd = (() =>
            {
                //gameController.SetFuel(-20);
                //gameController.SetKarma(-20);
                gameController.CurrentEvent = GetEventByID("holy_knights_options");
            })

        });
        events.Add(new Event()
        {
            Id = "holy_knights_options",
            TopText = "どうしようか？",
            OptionIDs = new List<(string, string)>()
            {
                ("罰金を払う\n(<color=red>所持金が半減</color>, <color=blue>カルマ値-50</color>)", "holy_knights_result001"),
                ("無視して、強引に進む\n(<color=red>戦闘に突入</color>, <color=blue>カルマ値+10</color>)" , "holy_knights_result002"),
                ("ワイロを渡し、ごまかす\n(確率で結果が変わる)", "holy_knights_result003")
            },
        });
        events.Add(new Event()
        {
            Id = "holy_knights_result001",
            Texts = new List<string>()
            {
                "冒険者たちは大人しく従う事にした。\n",
                "「うむ、それでいいんだ。もう面倒は起こすなよ」彼らはそう言って去っていった。\n" +
                "<color=yellow>燃料-20、カルマ値-20</color>"
            },
            OnEnd = (() =>
            {
                //gameController.SetFuel(-20);
                //gameController.SetKarma(-20);
                gameController.AllyManager.AddExps(1000);
                gameController.CurrentEvent = null;
            })
        });
        events.Add(new Event()
        {
            Id = "holy_knights_result002",
            Texts = new List<string>()
            {
                "冒険者たちは、それぞれ武器を手にとった。\n",
                "「ほう…我々に歯向かうとはな。後悔するなよ？」"
            },
            OnEnd = (() =>
            {
                //gameController.SetFuel(-20);
                //gameController.SetKarma(-20);
                gameController.CurrentEvent = null;
            })
        });
        events.Add(new Event()
        {
            Id = "holy_knights_result003",
            Texts = new List<string>()
            {
            },
            OnEnd = (() =>
            {
                gameController.CurrentEvent = GetRandomResult("holy_knights_result003");
            })
        });
        events.Add(new Event()
        {
            Id = "holy_knights_result004",
            Texts = new List<string>()
            {
                "ワイロを渡すことに成功した。"
            },
            OnEnd = (() =>
            {
                gameController.CurrentEvent = null;
            })
        });
        events.Add(new Event()
        {
            Id = "holy_knights_result005",
            Texts = new List<string>()
            {
                "ワイロを渡すことに失敗した。"
            },
            OnEnd = (() =>
            {
                gameController.CurrentEvent = null;
            })
        });
        #endregion
    }

    public Event GetRandomResult(string id)
    {
        var random = UnityEngine.Random.Range(0, 100);
        switch (id)
        {
            case "holy_knights_result003":
                if (random > 30)
                {
                    return GetEventByID("holy_knights_result004");
                }
                else
                {
                    return GetEventByID("holy_knights_result005");
                }
            default:
                return null;
        }
    }

    public void OnEnd(string id)
    {
        switch (id)
        {
            case "hungry_human_result01":
                gameController.SetKarma(-20);
                break;
            default:
                break;
        }
    }

    private List<string> GetOptions(string id)
    {
        return null;
    }

    public void GetRandomItem()
    {

    }

}
