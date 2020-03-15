package com.example.segurados.realm;

import android.app.Application;

import io.realm.Realm;
import io.realm.RealmConfiguration;

public class SeguradosApp extends Application {
    @Override
    public void onCreate() {
        super.onCreate();
        Realm.init(this);

        RealmConfiguration config = new RealmConfiguration.Builder()
                //.name("segurados.realm")
               // .schemaVersion(0)
                .build();
        Realm.setDefaultConfiguration(config);

    }
}
