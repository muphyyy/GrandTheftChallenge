$('#loginButton').click(() => {
    mp.trigger('LoginServer', $('#login').val(), $('#password').val());
});

$('#registerButton').click(() => {
    mp.trigger('RegisterServer', $('#login').val(), $('#email').val(), $('#password').val());
});
