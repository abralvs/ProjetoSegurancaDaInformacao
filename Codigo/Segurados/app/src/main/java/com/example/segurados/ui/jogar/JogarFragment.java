package com.example.segurados.ui.jogar;


import android.os.Bundle;

import androidx.fragment.app.Fragment;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.example.segurados.R;

/**
 * A simple {@link Fragment} subclass.
 */
public class JogarFragment extends Fragment {


    public JogarFragment() {
        // Required empty public constructor
    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View v = inflater.inflate(R.layout.fragment_jogar, container, false);



        return v;
    }

}
