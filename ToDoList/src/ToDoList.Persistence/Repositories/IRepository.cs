using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Domain.Models;

namespace ToDoList.Persistence.Repositories
{
    public interface IRepository<T> where T : class
    {
        public void Create(T item);

        public T ReadById(int id);

        public List<T> Read();

        public bool IdExists(int id);

        public void UpdateById(int id, T item);

        public void DeleteById(int id);

        /*Když se na to tak dívám, tak si nejsem vůbec jistá, jestli tu má co dělat ta pomocná metoda IdExists
        Asi mi to přišlo elegantnejší mít to na jednom místě, než to řešit v put a delete
        A vlastně zbytečně vracet null item do controlleru
        ale zase jestli byl účel maximálně využít generiku, tak mi vlastně generika ve dvou metodách teď úplně chybí
        a nevím, jestli je to správně :D
        Na druhou stranu proč vracet item v put a delete, když to nebylo v zadání
        Na třetí stranu, v zadání to sice nebylo, ale mohl by přijít změnový požadavek a pak je asi lepší mít iRepository obecný a upravovat jen controller?
        */

        /*
        Je pravda ze IdExists by som uplne v IRepository necakala, nebyva to klasicky metodou IRepository.
        Skor by som si upravila Update a Delete tak, aby som vedela, ci vobec nieco tieto metody modifikovali, bez nejakej pomocnej metody IdExists.
        Ta prakticky len vola to iste co ReadById, akurat vracia bool podla toho, ci sa nieco naslo alebo nie.
        Dalsi problem tohto vidim v tom ze Update a Delete uz nemaju validaciu na to, ci ten item existuje a rovno robia zmeny. Programator si teda vzdy musi pamatat, ze s Update a Delete musi volat aj IdExists, co su zbytocne dve volania namiesto jedneho, zaroven ak zabudne pouzit IdExists tak sa to cele pokazi (a chyby sa stavaju casto v takychto pripadoch :)
        V oboch IdExists a Update/Delete zaroven volas Find, co su len dalsie vyssie naklady na databazu (v pripade, ze prvok existuje, tak vlastne 2krat pristupujes do databazy aby si si ho precitala)
        Odporucam bud to teda prepisat do verzie, aku dali lektori do template - ak sa nenajde item, vyhodi sa specificka vynimka, ktora sa potom odchyti v controlleri.
        Alebo este je jedna moznost, ktoru som aj videla v praxi, a to vracat pri Update a Delete bool -> ak sa nenajde item, vrati sa false, inak sa modifikuje a vrati sa true.
        */
    }
}
