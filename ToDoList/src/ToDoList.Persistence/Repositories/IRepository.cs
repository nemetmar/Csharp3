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
    }
}
