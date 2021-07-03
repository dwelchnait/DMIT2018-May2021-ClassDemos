using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additonal Namespaces
using ChinookSystem.BLL;
using ChinookSystem.ViewModels;

#endregion

namespace WebApp.SamplePages
{
    public partial class ManagePlaylist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TracksSelectionList.DataSource = null;
        }

        #region Error Handling
        protected void SelectCheckForException(object sender,
                            ObjectDataSourceStatusEventArgs e)
        {
            MessageUserControl.HandleDataBoundException(e);
        }

        protected void InsertCheckForException(object sender,
                    ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                MessageUserControl.ShowInfo("Process success", "Album has been addded");
            }
            else
            {
                MessageUserControl.HandleDataBoundException(e);
            }
        }
        protected void UpdateCheckForException(object sender,
                ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                MessageUserControl.ShowInfo("Process success", "Album has been update");
            }
            else
            {
                MessageUserControl.HandleDataBoundException(e);
            }
        }
        protected void DeleteCheckForException(object sender,
                ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                MessageUserControl.ShowInfo("Process success", "Album has been removed");
            }
            else
            {
                MessageUserControl.HandleDataBoundException(e);
            }
        }
        #endregion


        protected void ArtistFetch_Click(object sender, EventArgs e)
        {
            TracksBy.Text = "Artist";
            if(string.IsNullOrEmpty(ArtistName.Text))
            {
                MessageUserControl.ShowInfo("You did not supply an artist name");
                //the value parameter field is a HiddenField
                //access to a HiddenField is by .Value NOT .Text
                SearchArg.Value = "zxcvg";  // this is simply a garbage value
            }
            else
            {
                SearchArg.Value = ArtistName.Text;
            }
            //to force the re-execution of an ODS attached to a display control
            //  rebind the display control
            TracksSelectionList.DataBind();
          }


        protected void GenreFetch_Click(object sender, EventArgs e)
        {
            TracksBy.Text = "Genre";
            //in this example this is NO prompt line on the ddl.
            //this means that no validation for selection needs to be done
            SearchArg.Value = GenreDDL.SelectedValue;
            TracksSelectionList.DataBind();
        }

        protected void AlbumFetch_Click(object sender, EventArgs e)
        {
            TracksBy.Text = "Album";
            if (string.IsNullOrEmpty(AlbumTitle.Text))
            {
                MessageUserControl.ShowInfo("You did not supply an album title");
                //the value parameter field is a HiddenField
                //access to a HiddenField is by .Value NOT .Text
                SearchArg.Value = "zxcvg";  // this is simply a garbage value
            }
            else
            {
                SearchArg.Value = AlbumTitle.Text;
            }
            //to force the re-execution of an ODS attached to a display control
            //  rebind the display control
            TracksSelectionList.DataBind();
        }

        protected void PlayListFetch_Click(object sender, EventArgs e)
        {
            //code to go here
            //username is coming from the system  via security
            //since security has yet to be installed, a default will be
            //  setup for the username value
            string username = "HansenB";

            //validate data present
            if (string.IsNullOrWhiteSpace(PlaylistName.Text.Trim()))
            {
                MessageUserControl.ShowInfo("Playlist Search",
                    "No playlist name was supplied");
            }
            else
            {
                //do a standard lookup
                //assign results to a gridview
                //use some user friendly error handling
                //the way we are doing the error handling is using
                //  MessageUserControl instead of try/catch
                //MessageUserControl has the try/catch embedded within
                //  the control logic
                //Within the control there exists a method called .TryRun()
                //syntax:
                //  MessageUserControl.TryRun( () => {
                //
                //          your coding logic
                //
                //  }[, "Message title", "success message"]);
                //
                MessageUserControl.TryRun(() =>
                {
                    //your code
                    PlaylistTracksController sysmgr = new PlaylistTracksController();
                    //the attachment of the playlist to the web control
                    //      will be used throughout the web page; as such
                    //      the code for refreshing is in its own method
                    RefreshPlaylist(sysmgr, username);
                },"Playlist Search","View the requested playlist below.");
            }
        }

        protected void RefreshPlaylist(PlaylistTracksController sysmgr, string username)
        {
            List<UserPlaylistTrack> info = sysmgr.List_TracksForPlaylist(PlaylistName.Text, username);
            PlayList.DataSource = info;
            PlayList.DataBind();
        }

        protected void MoveDown_Click(object sender, EventArgs e)
        {
            //code to go here
 
        }

        protected void MoveUp_Click(object sender, EventArgs e)
        {
            //code to go here
 
        }

        protected void MoveTrack(int trackid, int tracknumber, string direction)
        {
            //call BLL to move track
 
        }


        protected void DeleteTrack_Click(object sender, EventArgs e)
        {
            //code to go here
 
        }

        protected void TracksSelectionList_ItemCommand(object sender, 
            ListViewCommandEventArgs e)
        {
            string username = "HansenB"; // until security is implemented

            //form event validation: Presence
            if (string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageUserControl.ShowInfo("Missing Data", "Enter a playlist name.");
            }
            else
            {
                //access the contents of a control on the selected Listview row
                string song = (e.Item.FindControl("Namelabel") as Label).Text;
                int trackid = int.Parse(e.CommandArgument.ToString());

                MessageUserControl.TryRun(() => {
                    PlaylistTracksController sysmgr = new PlaylistTracksController();
                    sysmgr.Add_TrackToPLaylist(PlaylistName.Text, username, trackid, song);
                    RefreshPlaylist(sysmgr, username);
                },"Add Track to Playlist","Track has been added to the playlist");

            }
            
        }

    }
}