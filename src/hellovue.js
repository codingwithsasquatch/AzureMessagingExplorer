var hello = require('hellojs/dist/hello.all.min.js');

hello.init({
  google: 'GOOGLE_APP_CLIENT_ID',
  facebook: 'FACEBOOK_APP_CLIENT_ID',
  microsoft: 'MICROSOFT_CLIENT_ID'
}, {
  redirect_uri: 'redirect.html'
});

const hellovue = {

  install: function (Vue, options) {

    Vue.hello = hello;
  }
};

export default hellovue;