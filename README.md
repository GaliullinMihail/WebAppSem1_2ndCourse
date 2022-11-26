В консоле:
    start -> запустить сервер на http://localhost:7777/
    stop -> остановить сервер
    restart -> перезапускает сервер
    end -> завершает работу программы

На Сервере:

    http://localhost:7777/ - основная страница для перехода 
    
    http://localhost:7777/registration -
        * логин, пароль и его соотвествие проверяется js
        * Check login - с помощью ajax проверяет запросом есть ли в базе логин, написанный в поле login
        
    http://localhost:7777/authorisation -
        * логин пароль проверяется на сервере
        
    http://localhost:7777/sites -
        * есть доступ не авторизованным
        * авторизованным можно добавить сайт в список сайтов
        * авторизованным можно добавить сайт в избранное
        
    http://localhost:7777/posts -
        * есть доступ не авторизованным
        * авторизованным можно добавить пост в список постов(добавить можно только те, которые уже не       хранятся в избранном у данного пользователя)
        * авторизованным можно изменить пост если он был написан этим пользователем(edit posts)
        
    http://localhost:7777/genres -
        * есть доступ не авторизованным
        
    http://localhost:7777/profile
        * Exit Profile - выйти из профиля
        
    http://localhost:7777/favorite -
        * есть доступ только авторизованным пользователям
        * можно удалить из избранного (Delete From Favorite)
