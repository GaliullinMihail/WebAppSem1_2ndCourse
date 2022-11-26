function viewDiv(){
    let posts = document.querySelectorAll('.EditPost'),
        buttons = document.querySelectorAll('.editButton');
    posts.forEach(function(post) {
        post.style.display = 'block';
    })
    buttons.forEach(function(button) {
        button.style.display = 'none';
    })
};